package viseo.wiiwars.Service;

import android.os.AsyncTask;
import android.widget.Toast;

import com.google.android.gms.wearable.MessageEvent;
import com.google.android.gms.wearable.WearableListenerService;
import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.table.MobileServiceTable;

import org.apache.http.HttpResponse;
import org.apache.http.auth.UsernamePasswordCredentials;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.auth.BasicScheme;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.IOException;
import java.net.MalformedURLException;
import java.util.List;
import java.util.concurrent.ExecutionException;

import viseo.wiiwars.Model.Saber;

import static com.microsoft.windowsazure.mobileservices.table.query.QueryOperations.val;

/**
 * Created by SCH3373 on 25/03/2015.
 */
public class ListenerService extends WearableListenerService {

    private final static String baseAddress = "https://viseo-wii-wars-dev-noeu-mobilesrv.azure-mobile.net/";

    private final static String turnOnUrl=baseAddress+"api/saber/TurnOn/1";
    private final static String turnOffUrl=baseAddress+"api/saber/TurnOff/1";
    private final static String colorRedUrl=baseAddress+"api/saber/ChangeColorRed/1";
    private final static String colorBlueUrl=baseAddress+"api/saber/ChangeColorBlue/1";
    private final static String colorGreenUrl=baseAddress+"api/saber/ChangeColorGreen/1";

    private final static String apiKey="kAFbTILxKueFrdqAvhsuaaAAgdXLub62";

    private MobileServiceClient mClient;

    private MobileServiceTable<Saber> saberTable;

    private Saber currentSaber;

    @Override
    public void onCreate() {
        super.onCreate();
        try {

            mClient = new MobileServiceClient(
                    baseAddress,
                    apiKey,
                    this);
        } catch (MalformedURLException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onMessageReceived(MessageEvent messageEvent) {
        showToast(messageEvent.getPath());
        processMessage(messageEvent.getPath());
    }

    public void showToast(String message)
    {
        Toast.makeText(this,message,Toast.LENGTH_SHORT).show();
    }

    private void processMessage(String message) {
        switch(message)
        {
            case "on":
                //turnOn();
                callUrl(turnOnUrl);
                break;
            case "off":
                //turnOff();
                callUrl(turnOffUrl);
                break;
            case "green":
                callUrl(colorGreenUrl);
                break;
            case "blue":
                callUrl(colorBlueUrl);
                break;
            case "red":
                callUrl(colorRedUrl);
                break;
        }
    }

    private void callUrl(String url)
    {
        System.out.println(url);
        try {
            HttpGet httpGet = new HttpGet(url);
            httpGet.addHeader(BasicScheme.authenticate(
                    new UsernamePasswordCredentials("", apiKey),
                    "UTF-8", false));
            DefaultHttpClient httpClient = new DefaultHttpClient();
            httpClient.execute(httpGet);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
