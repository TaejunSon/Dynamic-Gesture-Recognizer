# Dynamic Gesture Recognizer

in Scenes -> SampleScene

**built in hand tracker**
OVRCameraRig -> TrackingSpace -> LeftHandAnchor / RightHandAnchor 

1. Passthrough
Quest 3 mr system
Tutorial : https://www.youtube.com/watch?v=D8_vdJG0UZ8

2. Cube (1) 
Tracking system for Hand Tip

![Untitled (5)](https://github.com/user-attachments/assets/a046564a-2e96-4926-a99e-d8d2604e2d8e)

figure 1. built in hand tracker bone name 

ref : https://developer.oculus.com/documentation/unity/unity-handtracking/?locale=fr_FR

2-1. HandTrackingScrip.cs
Cube (1) 

track the thumb finger tip, and store fingers position history. 

2-2. SendAndReceive.cs

Send : thumb finger position x, y 
Receive : Gesture recognize result 

2-3. ObjectCollision.cs

Detecting cube collision checking

3. IndexFingerTrackingCube

Tracking index finger tip 
