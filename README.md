# Dynamic Gesture Recognizer

**[1] Two hand model in project **
1. OVRCameraRig -> TrackingSpace -> RightHandAnchor
From this model extract hand skeleton data and by using this, tracking index finger tip and make history of index finger tip

2. OcculusInteractionSampleRig -> InputOVR -> Hands
This one used by Static Pose check and General Grab. but in this project we use redefined Grab gesture, so just only using by Static Pose Check

**[2] Cube (1)**
This cube is used by tracking right hand index finger. 
It has two functions (1) Hand tracking script (2) Send And Receive

(1) Hand tracking script
We use this script for tracking index finger tip and record history of index finger tip's movement. 
We can visualize history of index finger tip by prefabbed spheres.

(2) Send And Receive
We use this script for sending and receiving data. 
We send index finger tip's x position and y position data by TCP per 0.03s (because we model pretrained by 30fps, but Quest3 using commputer power sustain 72 fps). 
The server (which is edited by python) will receive data. and each taking 8 datas it interpret by pretrained model. 
Then client receive the interpret result then visulize the result.

**[3] Cylinder and Sphere**
We have two object whici can grab and interact by dynamic hand gesture. 

Cylinder - up / down / left / right
Sphere - Clockwise / CounterClockwise

Each one can be grabbed by collider collision (object and hand) and resolve grab by Static Pose Detecting (which is scissor pose) 
