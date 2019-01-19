# Y-Vision
Y-Vision is a real-time robust crowd tracking proof-of-concept framework which combines RGB+D data from multiple kinects. A short demo is available here:

[![Y-Vision youtube demo](https://img.youtube.com/vi/uKAcJboXMJ8/0.jpg)](https://www.youtube.com/watch?v=uKAcJboXMJ8)

## Features
1. RGB and depth specific features extraction, detection and fusion.
2. HOG features and blobs from connect component labeling are used as features.
3. Bayesian model for pedestrian detection.
4. Tracking using fast branch and bound association.
5. Ground tracking using simple plane estimation (requires calibration)
6. TCP interface with simple client API.
7. Data set collection tool
8. Efficient Kinect data capture

## Packages
1. **Y-Vision** is a C# pipeline for pedestrian detection and tracking.
2. **CollectionTool** allows for massive data collection in the wild using a Kinect , useful for machine learning.
3. **Y-TcpServer** is a TCP server that streams info on the detected pedestrians in real time.
4. **Y-TcpClient** and Y-API are self-contained libraries allowing easy integration in other applications (e.g. unity)
5. **Y-TestApi2d** listens and displays pedestrian detection events.
6. **Y-CalibrationBoard** is a GUI calibration environment to help setup the Kinect rig.
7. **Y-Emulator** is a development tool that simulates pedestrians for when no Kinect are available.
8. **Y-MachineLearning** contains the Bayesian model for the pedestrian detection
9. **Y-UnitTests** is the test package.
10. **Y-Visualization** is a set of helpers to display the results and debug.


## License
Y-Vision is licensed under the GNU 3.0 GENERAL PUBLIC license.
