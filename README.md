# unity-camera-set

To enable camera movement add MouseLook script (it handles input) and any BaseCameraMover derived script(for example FreeCameraMover) to your camera.

To switch camera movers you have to enable your desired cameraMover script component for mainCamera and make shure that desired component is above other components(derived by BaseCameraMover)
