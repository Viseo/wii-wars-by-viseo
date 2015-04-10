package viseo.wiiwars.Fragment;

import android.app.Activity;
import android.app.Fragment;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.speech.RecognitionListener;
import android.speech.RecognizerIntent;
import android.speech.SpeechRecognizer;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CompoundButton;
import android.widget.Switch;

import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.wearable.Node;
import com.google.android.gms.wearable.NodeApi;
import com.google.android.gms.wearable.Wearable;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

import viseo.wiiwars.R;

/**
 * Created by SCH3373 on 11/03/2015.
 */
public class TurnOnFragment extends Fragment implements RecognitionListener{

    private Activity activity;
    private Switch turnOnSwitch;
    private SpeechRecognizer speech = null;
    private Intent recognizerIntent;

    private String nodeId;
    private static long CONNECTION_TIME_OUT_MS = 30000;

    private static final int SPEECH_REQUEST_CODE = 0;

    @Override
    public void onAttach(Activity activity)
    {
        super.onAttach(activity);
        this.activity = activity;
    }

    @Override
    public void onActivityCreated(Bundle savedInstanceState){
        super.onActivityCreated(savedInstanceState);

        retrieveDeviceNode();

        turnOnSwitch = (Switch) getView().findViewById(R.id.turnOnSwitch);
        turnOnSwitch.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    sendMessage("on");
                }
                else {
                    sendMessage("off");
                }
            }
        });

//        speech = SpeechRecognizer.createSpeechRecognizer(this.getActivity());
//        speech.setRecognitionListener(this);
//        recognizerIntent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
//        recognizerIntent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_PREFERENCE,
//                "en");
//        recognizerIntent.putExtra(RecognizerIntent.EXTRA_CALLING_PACKAGE,
//                this.getActivity().getPackageName());
//        recognizerIntent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL,
//                RecognizerIntent.LANGUAGE_MODEL_WEB_SEARCH);
//        recognizerIntent.putExtra(RecognizerIntent.EXTRA_MAX_RESULTS, 3);
//        speech.startListening(recognizerIntent);

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        return inflater.inflate(R.layout.turn_on_fragment, container, false);
    }

    private void retrieveDeviceNode() {
        final GoogleApiClient client = getGoogleApiClient(this.getActivity());
        new Thread(new Runnable() {
            @Override
            public void run() {
                client.blockingConnect(CONNECTION_TIME_OUT_MS, TimeUnit.MILLISECONDS);
                NodeApi.GetConnectedNodesResult result =
                        Wearable.NodeApi.getConnectedNodes(client).await();
                List<Node> nodes = result.getNodes();
                if (nodes.size() > 0) {
                    nodeId = nodes.get(0).getId();
                }
                client.disconnect();
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
        final GoogleApiClient client = getGoogleApiClient(this.getActivity());
        if (nodeId != null) {
            new Thread(new Runnable() {
                @Override
                public void run() {
                    client.blockingConnect(CONNECTION_TIME_OUT_MS, TimeUnit.MILLISECONDS);
                    Wearable.MessageApi.sendMessage(client, nodeId, message, null);
                    client.disconnect();
                }
            }).start();
        }
    }

    @Override
    public void onReadyForSpeech(Bundle params) {
        System.out.println("ready for speech");
    }

    @Override
    public void onBeginningOfSpeech() {
        System.out.println("beginning of speech");
    }

    @Override
    public void onRmsChanged(float rmsdB) {
        System.out.println("rms changed");
    }

    @Override
    public void onBufferReceived(byte[] buffer) {
        System.out.println("buffer received");
    }

    @Override
    public void onEndOfSpeech() {
        System.out.println("end of speech");
    }

    @Override
    public void onError(int error) {
        System.out.println("Error : "+getErrorText(error));
    }

    @Override
    public void onResults(Bundle results) {

        ArrayList<String> matches = results
                .getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);
        String text = "";
        for (String result : matches)
            text += result + "\n";

        System.out.println(text);
        sendMessage(text.toLowerCase());
    }

    @Override
    public void onPartialResults(Bundle partialResults) {
        System.out.println("partial results");
    }

    @Override
    public void onEvent(int eventType, Bundle params) {
        System.out.println("on event");
    }

    public static String getErrorText(int errorCode) {
        String message;
        switch (errorCode) {
            case SpeechRecognizer.ERROR_AUDIO:
                message = "Audio recording error";
                break;
            case SpeechRecognizer.ERROR_CLIENT:
                message = "Client side error";
                break;
            case SpeechRecognizer.ERROR_INSUFFICIENT_PERMISSIONS:
                message = "Insufficient permissions";
                break;
            case SpeechRecognizer.ERROR_NETWORK:
                message = "Network error";
                break;
            case SpeechRecognizer.ERROR_NETWORK_TIMEOUT:
                message = "Network timeout";
                break;
            case SpeechRecognizer.ERROR_NO_MATCH:
                message = "No match";
                break;
            case SpeechRecognizer.ERROR_RECOGNIZER_BUSY:
                message = "RecognitionService busy";
                break;
            case SpeechRecognizer.ERROR_SERVER:
                message = "error from server";
                break;
            case SpeechRecognizer.ERROR_SPEECH_TIMEOUT:
                message = "No speech input";
                break;
            default:
                message = "Didn't understand, please try again.";
                break;
        }
        return message;
    }
}
