package viseo.wiiwars.Service;

import com.google.android.gms.wearable.MessageEvent;
import com.google.android.gms.wearable.WearableListenerService;

import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.IOException;

/**
 * Created by SCH3373 on 25/03/2015.
 */
public class ListenerService extends WearableListenerService {

    private final static String baseAddress = "http://inv010581:9000";

    private final static String turnOnUrl=baseAddress+"/api/saber/TurnOn/1";
    private final static String turnOffUrl=baseAddress+"/api/saber/TurnOff/1";
    private final static String colorRedUrl=baseAddress+"/api/saber/ChangeColorRed/1";
    private final static String colorBlueUrl=baseAddress+"/api/saber/ChangeColorBlue/1";
    private final static String colorGreenUrl=baseAddress+"/api/saber/ChangeColorGreen/1";

    @Override
    public void onMessageReceived(MessageEvent messageEvent) {
        processMessage(messageEvent.getPath());
    }

    private void processMessage(String message) {
        switch(message)
        {
            case "on":
                callUrl(turnOnUrl);
                break;
            case "off":
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
        try {
            HttpGet httpGet = new HttpGet(url);
            DefaultHttpClient httpClient = new DefaultHttpClient();
            httpClient.execute(httpGet);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
