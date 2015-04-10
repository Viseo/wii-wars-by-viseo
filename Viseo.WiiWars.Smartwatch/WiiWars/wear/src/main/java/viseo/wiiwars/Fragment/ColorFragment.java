package viseo.wiiwars.Fragment;

import android.app.Activity;
import android.app.Fragment;
import android.content.Context;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.wearable.Node;
import com.google.android.gms.wearable.NodeApi;
import com.google.android.gms.wearable.Wearable;

import java.util.List;
import java.util.concurrent.TimeUnit;

import viseo.wiiwars.R;

/**
 * Created by SCH3373 on 10/03/2015.
 */
public class ColorFragment extends Fragment{

    private TextView mTextView;
    private Button btnRed, btnGreen, btnBlue;
    private View inflatedView;
    private Activity activity;

    private String nodeId;
    private static long CONNECTION_TIME_OUT_MS = 30000;

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

        mTextView = (TextView) getView().findViewById(R.id.text);
        btnRed = (Button) getView().findViewById(R.id.btnRed);
        btnBlue = (Button) getView().findViewById(R.id.btnBlue);
        btnGreen = (Button) getView().findViewById(R.id.btnGreen);
        btnRed.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                System.out.println("btn red !");
                sendMessage("red");
            }
        });
        btnBlue.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                sendMessage("blue");
            }
        });
        btnGreen.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                sendMessage("green");
            }
        });
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        return inflater.inflate(R.layout.color_fragment, container, false);
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
}
