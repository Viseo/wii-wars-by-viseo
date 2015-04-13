package viseo.wiiwars.Model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by SCH3373 on 13/04/2015.
 */
public class Saber {

    @SerializedName("color")
    private String color;

    @SerializedName("id")
    private String id;

    @SerializedName("power")
    private boolean power;

    public Saber() {

    }

    public Saber(String color,boolean power) {
        this.color=color;
        this.power=power;
    }

    public String getColor() {
        return color;
    }

    public void setColor(String color) {
        this.color = color;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public boolean isPower() {
        return power;
    }

    public void setPower(boolean power) {
        this.power = power;
    }
}
