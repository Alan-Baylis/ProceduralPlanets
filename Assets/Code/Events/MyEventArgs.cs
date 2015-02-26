/* Purpose: This class is an EventArgs used in all events.
 * 
 * Special Notes: Used by events when the're thrown.
 * 
 * Author: Devyn Cyphers; Devcon.
 */

using System;

public class MyEventArgs : EventArgs {

    // Variables to use.
    private string EventInfo;

    public MyEventArgs(string Text) {
        EventInfo = Text;
    }

    // Returns the eventInfo.
    public string GetInfo() {
        return EventInfo;
    }
}