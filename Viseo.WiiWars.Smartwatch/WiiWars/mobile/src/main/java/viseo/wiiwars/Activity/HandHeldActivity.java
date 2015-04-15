package viseo.wiiwars.Activity;

import android.app.AlertDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.pm.ActivityInfo;
import android.content.res.Configuration;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.media.MediaPlayer;
import android.os.AsyncTask;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.v7.app.ActionBarActivity;
import android.util.FloatMath;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.wearable.MessageApi;
import com.google.android.gms.wearable.MessageEvent;
import com.google.android.gms.wearable.Wearable;

import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.IOException;

import viseo.wiiwars.R;
import viseo.wiiwars.Service.WebServiceScore;


public class HandHeldActivity extends ActionBarActivity implements ApiAddressDialogFragment.NoticeDialogListener, MessageApi.MessageListener, GoogleApiClient.ConnectionCallbacks, GoogleApiClient.OnConnectionFailedListener, SensorEventListener {

    private GoogleApiClient client;
    private MediaPlayer mPlayer = null;
    private SensorManager sm;
    private Sensor mAccelerometer;
    private ImageView lightSaber,lightSaberOff;
    private String color="red";
    private String shape="simple";
    private boolean power=false;

    private String apiBaseAddress = "";

    private String turnOnUrl=apiBaseAddress+"/api/saber/TurnOn/1";
    private String turnOffUrl=apiBaseAddress+"/api/saber/TurnOff/1";
    private String colorRedUrl=apiBaseAddress+"/api/saber/ChangeColorRed/1";
    private String colorBlueUrl=apiBaseAddress+"/api/saber/ChangeColorBlue/1";
    private String colorGreenUrl=apiBaseAddress+"/api/saber/ChangeColorGreen/1";

    private float[] mGravity;
    private float mAccel;
    private float mAccelCurrent;
    private float mAccelLast;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_hand_held);

        SharedPreferences sharedPref = PreferenceManager.getDefaultSharedPreferences(this);
        this.apiBaseAddress = sharedPref.getString("address","http://");

        client = new GoogleApiClient.Builder(this)
                .addApi(Wearable.API)
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
                .build();

        lightSaber = (ImageView) findViewById(R.id.lightSaberImage);
        lightSaberOff = (ImageView) findViewById(R.id.lightSaberOff);
        lightSaberOff.setImageDrawable(getResources().getDrawable(R.drawable.lightsaber_off));

        //Sensor manager
        sm = (SensorManager)getSystemService(SENSOR_SERVICE);
        mAccelerometer = sm.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
        mAccel = 0.00f;
        mAccelCurrent = SensorManager.GRAVITY_EARTH;
        mAccelLast = SensorManager.GRAVITY_EARTH;

        lockScreenOrientation();

        //WebServiceScore ws = new WebServiceScore(this);
        //ws.execute();
    }

    @Override
    protected void onStart() {
        super.onStart();
        client.connect();
    }

    @Override
    protected void onStop() {
        if (null != client && client.isConnected()) {
            Wearable.MessageApi.removeListener(client, this);
            client.disconnect();
        }
        super.onStop();
    }

    @Override
    protected void onResume() {
        super.onResume();
        sm.registerListener(this, mAccelerometer, SensorManager.SENSOR_DELAY_NORMAL);
    }

    @Override
    public void onConnected(Bundle bundle) {
        Wearable.MessageApi.addListener(client, this);
    }

    @Override
    public void onConnectionSuspended(int i) {
        System.out.println("Connection paused");
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        client.disconnect();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_hand_held, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }
        if (id == R.id.action_changeAddress) {
            ApiAddressDialogFragment dialog = new ApiAddressDialogFragment();
            dialog.show(this.getFragmentManager(), "Change address");
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onDialogPositiveClick(DialogFragment dialog) {
        Dialog dialogView = dialog.getDialog();
        TextView t = (TextView) dialogView.findViewById(R.id.tIpAddress);
        this.apiBaseAddress = String.valueOf(t.getText());
        this.turnOnUrl=apiBaseAddress+"/api/saber/TurnOn/1";
        this.turnOffUrl=apiBaseAddress+"/api/saber/TurnOff/1";
        this.colorRedUrl=apiBaseAddress+"/api/saber/ChangeColorRed/1";
        this.colorBlueUrl=apiBaseAddress+"/api/saber/ChangeColorBlue/1";
        this.colorGreenUrl=apiBaseAddress+"/api/saber/ChangeColorGreen/1";
        SharedPreferences sharedPref = PreferenceManager.getDefaultSharedPreferences(this);
        SharedPreferences.Editor editor1 = sharedPref.edit();
        editor1.putString("address",this.apiBaseAddress);
        editor1.apply();
    }

    @Override
    public void onDialogNegativeClick(DialogFragment dialog) {

    }

    private void callUrl(final String url)
    {
        System.out.println(url);
        new AsyncTask<Void, Void, Void>(){
            @Override
            protected Void doInBackground(Void... params) {
                try {
                    HttpGet httpGet = new HttpGet(url);
                    DefaultHttpClient httpClient = new DefaultHttpClient();
                    httpClient.execute(httpGet);
                } catch (IOException e) {
                    e.printStackTrace();
                }
                return null;
            }
        }.execute();
    }

    @Override
    public void onMessageReceived(final MessageEvent messageEvent) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                processMessage(messageEvent.getPath());
            }
        });
    }

    private void processMessage(String message)
    {
        switch(message)
        {
            case "red":
                this.color="red";
                //callUrl(colorRedUrl);
                break;

            case "blue":
                this.color="blue";
                //callUrl(colorBlueUrl);
                break;

            case "green":
                this.color="green";
                //callUrl(colorGreenUrl);
                break;

            case "simple":
                this.shape="simple";
                break;

            case "double":
                this.shape="double";
                break;

            case "sw7":
                this.shape="sw7";
                break;

            case "on":
                this.power=true;
                //callUrl(turnOnUrl);
                //playSound(R.raw.lightsaberin);
                break;

            case "off":
                this.power=false;
                //callUrl(turnOffUrl);
                //playSound(R.raw.lightsaberout);
                break;
        }
        updateLightSaber();
    }

    private void updateLightSaber() {
        if (color.equals("red")) {
            if (shape.equals("simple")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.lightsaber_red));
            } else if (shape.equals("double")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.doublelightsaber_red));
            } else if (shape.equals("sw7")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.sw7lightsaber_red));
            }
        } else if (color.equals("blue")) {
            if (shape.equals("simple")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.lightsaber_blue));
            } else if (shape.equals("double")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.doublelightsaber_blue));
            } else if (shape.equals("sw7")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.sw7lightsaber_blue));
            }
        } else if (color.equals("green")) {
            if (shape.equals("simple")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.lightsaber_green));
            } else if (shape.equals("double")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.doublelightsaber_green));
            } else if (shape.equals("sw7")) {
                this.lightSaber.setImageDrawable(getResources().getDrawable(R.drawable.sw7lightsaber_green));
            }
        }
        if(power) {
            this.lightSaber.setVisibility(View.VISIBLE);
            this.lightSaberOff.setVisibility(View.INVISIBLE);
        }
        else {
            this.lightSaber.setVisibility(View.INVISIBLE);
            this.lightSaberOff.setVisibility(View.VISIBLE);
            if(shape.equals("double"))
                this.lightSaberOff.setImageDrawable(getResources().getDrawable(R.drawable.doublelightsaber_off));
            else
                this.lightSaberOff.setImageDrawable(getResources().getDrawable(R.drawable.lightsaber_off));
        }
    }

    @Override
    public void onPause() {
        super.onPause();
        if(mPlayer != null) {
            mPlayer.stop();
            mPlayer.release();
        }
        sm.unregisterListener(this);
    }

    private void playSound(int resId) {
        if(mPlayer != null) {
            mPlayer.stop();
            mPlayer.release();
        }
        mPlayer = MediaPlayer.create(this, resId);
        mPlayer.start();
    }

    @Override
    public void onConnectionFailed(ConnectionResult connectionResult) {
        System.out.println("connection failed");
    }

    @Override
    public void onSensorChanged(SensorEvent event) {

        switch (event.sensor.getType()) {

            case Sensor.TYPE_ACCELEROMETER:

                mGravity = event.values.clone();
                // Shake detection
                float x = mGravity[0];
                float y = mGravity[1];
                float z = mGravity[2];
                mAccelLast = mAccelCurrent;
                mAccelCurrent = FloatMath.sqrt(x * x + y * y + z * z);
                float delta = mAccelCurrent - mAccelLast;
                mAccel = mAccel * 0.9f + delta;
                // Make this higher or lower according to how much
                // motion you want to detect
                if(mAccel > 3){
                    if(power) {
                        //playSound(R.raw.lightsabervibration);
                    }
                }
                break;
        }
    }

    @Override
    public void onAccuracyChanged(Sensor sensor, int accuracy) {

    }

    private void lockScreenOrientation() {
        int currentOrientation = this.getResources().getConfiguration().orientation;
        if (currentOrientation == Configuration.ORIENTATION_PORTRAIT) {
            this.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
        } else {
            this.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
        }
    }
}
