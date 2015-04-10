package viseo.wiiwars.Service;

import android.content.Context;
import android.os.AsyncTask;

import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.wearable.Node;
import com.google.android.gms.wearable.NodeApi;
import com.google.android.gms.wearable.Wearable;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonParser;
import com.google.gson.stream.JsonReader;

import org.json.JSONArray;
import org.json.JSONException;

import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URL;
import java.net.URLConnection;
import java.util.List;
import java.util.concurrent.TimeUnit;

/**
 * Created by SCH3373 on 23/03/2015.
 */
public class WebServiceScore extends AsyncTask<Void,Void,Void>{

    private URLConnection urlConnection;
    private Context context;
    private String score;
    private final static String apiUrl="https://viseo-wii-wars-dev-noeu-websites.azurewebsites.net/api/saberscore";

    private String nodeId;
    private static long CONNECTION_TIME_OUT_MS = 30000;

    public WebServiceScore(Context context) {
        this.context=context;
        retrieveDeviceNode();
    }

    @Override
    protected void onPostExecute(Void aVoid) {
        super.onPostExecute(aVoid);
        sendMessage(score);
    }

    @Override
    protected Void doInBackground(Void... params) {
        callWebService();
        return null;
    }

    public void callWebService()
    {
        try {
            urlConnection = new URL(apiUrl).openConnection();
            urlConnection.connect();

            JsonReader reader = new JsonReader(new InputStreamReader(urlConnection.getInputStream()));
            JsonParser parser = new JsonParser();
            JsonElement rootElement = parser.parse(reader);
            JsonArray arrayJson = rootElement.getAsJsonArray();
            try {
                JSONArray array = new JSONArray(arrayJson.toString());
                score = array.getString(4).substring(12);
            } catch (JSONException e) {
                e.printStackTrace();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void retrieveDeviceNode() {
        final GoogleApiClient clientSend = getGoogleApiClient(this.context);
        new Thread(new Runnable() {
            @Override
            public void run() {
                clientSend.blockingConnect(CONNECTION_TIME_OUT_MS, TimeUnit.MILLISECONDS);
                NodeApi.GetConnectedNodesResult result =
                        Wearable.NodeApi.getConnectedNodes(clientSend).await();
                List<Node> nodes = result.getNodes();
                if (nodes.size() > 0) {
                    nodeId = nodes.get(0).getId();
                }
                clientSend.disconnect();
            }
        }).start();
    }

    private GoogleApiClient getGoogleApiClient(Context context) {
        return new GoogleApiClient.Builder(context)
                .addApi(Wearable.API)
                .build();
    }

    public void sendMessage(final String message)
    {
        System.out.println("message : "+message+" nodeId send : "+nodeId);
        final GoogleApiClient clientSend = getGoogleApiClient(this.context);
        if (nodeId != null) {
            new Thread(new Runnable() {
                @Override
                public void run() {
                    clientSend.blockingConnect(CONNECTION_TIME_OUT_MS, TimeUnit.MILLISECONDS);
                    Wearable.MessageApi.sendMessage(clientSend, nodeId, message, null);
                    clientSend.disconnect();
                }
            }).start();
        }
    }

}
