using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.KinectScripts;
//using System.Runtime.InteropServices;

public class FacetrackingManager : MonoBehaviour
{
    private class PlayerFacetrackingData
    {
        // The index of the player, whose face this manager tracks. Default is 0 (first player).
        public int playerIndex = 0;

        public Mesh mesh = null;

        public bool getFaceModelData = false;

        // whether the face model mesh was initialized
        public bool bFaceModelMeshInited = false;

        // Is currently tracking user's face
        public bool isTrackingFace = false;

        // Tolerance (in seconds) allowed to miss the tracked face before losing it
        public float faceTrackingTolerance = 0.25f;

        // The game object that will be used to display the face model mesh
        public GameObject faceModelMesh = new GameObject();

        // Public Bool to determine whether the model mesh should be mirrored or not
        public bool mirroredModelMesh = true;

        // returns true if the facetracking system is tracking a face
        public bool IsTrackingFace()
        {
            return isTrackingFace;
        }

        // primary user ID, as reported by KinectManager
        public long primaryUserID = 0;

        private float lastFaceTrackedTime = 0f;

        // Animation units
        public Dictionary<KinectInterop.FaceShapeAnimations, float> dictAU = new Dictionary<KinectInterop.FaceShapeAnimations, float>();
        public bool bGotAU = false;

        // Shape units
        public Dictionary<KinectInterop.FaceShapeDeformations, float> dictSU = new Dictionary<KinectInterop.FaceShapeDeformations, float>();
        public bool bGotSU = false;

        // Vertices of the face model
        public Vector3[] avModelVertices = null;
        public bool bGotModelVertices = false;

        // Head position and rotation
        public Vector3 headPos = Vector3.zero;
        public bool bGotHeadPos = false;

        public Quaternion headRot = Quaternion.identity;
        public bool bGotHeadRot = false;

        public void updatePlayer(bool facesUpdatedSuccessfully, KinectInterop.SensorData sensorData)
        {
            //replace primaryUserId with user1 and user2


            // update the face tracker
            if (facesUpdatedSuccessfully)
            {
                // estimate the tracking state
                isTrackingFace = sensorData.sensorInterface.IsFaceTracked(primaryUserID);

                // get the facetracking parameters
                if (isTrackingFace)
                {
                    lastFaceTrackedTime = Time.realtimeSinceStartup;

                    // get face rectangle
                    //bGotFaceRect = sensorData.sensorInterface.GetFaceRect(primaryUserID, ref faceRect);

                    // get head position
                    bGotHeadPos = sensorData.sensorInterface.GetHeadPosition(primaryUserID, ref headPos);

                    // get head rotation
                    bGotHeadRot = sensorData.sensorInterface.GetHeadRotation(primaryUserID, ref headRot);

                    // get the animation units
                    bGotAU = sensorData.sensorInterface.GetAnimUnits(primaryUserID, ref dictAU);

                    // get the shape units
                    bGotSU = sensorData.sensorInterface.GetShapeUnits(primaryUserID, ref dictSU);

                    Debug.Log("updateUser: " + playerIndex);

                    if (faceModelMesh != null && faceModelMesh.activeInHierarchy)
                    {
                        // apply model vertices to the mesh
                        if (!bFaceModelMeshInited)
                        {
                            CreateFaceModelMesh(sensorData);
                        }
                    }

                    if (getFaceModelData)
                    {
                        UpdateFaceModelMesh(sensorData);
                    }

                }
                else if ((Time.realtimeSinceStartup - lastFaceTrackedTime) <= faceTrackingTolerance)
                {
                    // allow tolerance in tracking
                    isTrackingFace = true;
                }

                if (faceModelMesh != null && bFaceModelMeshInited)
                {
                    faceModelMesh.SetActive(isTrackingFace);
                }
            }
        }

        private void CreateFaceModelMesh(KinectInterop.SensorData sensorData)
        {
            if (faceModelMesh == null)
                return;

            int iNumTriangles = sensorData.sensorInterface.GetFaceModelTrianglesCount();
            if (iNumTriangles <= 0)
                return;

            int[] avModelTriangles = new int[iNumTriangles];
            bool bGotModelTriangles = sensorData.sensorInterface.GetFaceModelTriangles(mirroredModelMesh, ref avModelTriangles);

            if (!bGotModelTriangles)
                return;

            int iNumVertices = sensorData.sensorInterface.GetFaceModelVerticesCount(0);
            if (iNumVertices < 0)
                return;

            avModelVertices = new Vector3[iNumVertices];
            bGotModelVertices = sensorData.sensorInterface.GetFaceModelVertices(0, ref avModelVertices);


            if (!bGotModelVertices)
                return;

            Vector2[] avModelUV = new Vector2[iNumVertices];

            //Quaternion faceModelRot = faceModelMesh.transform.rotation;
            //faceModelMesh.transform.rotation = Quaternion.identity;

            mesh = new Mesh();
            mesh.name = "FaceMesh";
            faceModelMesh.GetComponent<MeshFilter>().mesh = mesh;

            mesh.vertices = avModelVertices;

            mesh.uv = avModelUV;
            mesh.triangles = avModelTriangles;
            mesh.RecalculateNormals();



            //faceModelMesh.transform.rotation = faceModelRot;

            bFaceModelMeshInited = true;
        }

        private void UpdateFaceModelMesh(KinectInterop.SensorData sensorData)
        {
            // init the vertices array if needed
            if (avModelVertices == null)
            {
                int iNumVertices = sensorData.sensorInterface.GetFaceModelVerticesCount(primaryUserID);
                avModelVertices = new Vector3[iNumVertices];
            }

            // get face model vertices
            bGotModelVertices = sensorData.sensorInterface.GetFaceModelVertices(primaryUserID, ref avModelVertices);

            if (bGotModelVertices && faceModelMesh != null && bFaceModelMeshInited)
            {
                Quaternion faceModelRot = faceModelMesh.transform.rotation;
                //faceModelMesh.transform.rotation = Quaternion.identity;

                mesh = faceModelMesh.GetComponent<MeshFilter>().mesh;
                mesh.vertices = avModelVertices;
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();

                Debug.Log("Index " + playerIndex + "'s Mesh Vertices: " + mesh.vertices[10]);
            }
        }

        public Vector3 getMeshTransform()
        {
            Vector3 meshVector;
            if (isTrackingFace)
            {
                Debug.Log("getMeshTransform: " + playerIndex);
                meshVector = new Vector3(mesh.vertices[KinectVertices.getLefteyeInnercorner()].x,
                                         mesh.vertices[KinectVertices.getLefteyeInnercorner()].y,
                                         mesh.vertices[KinectVertices.getLefteyeInnercorner()].z);
            }
            else
                meshVector = new Vector3(0, 0, 0);

            return meshVector;
        }
    };

    [Range(1, 6)]
    public int players = 2;

    // The game object that will be used to display the face model mesh
    public GameObject[] faceModelMesh = null;

    PlayerFacetrackingData[] playerData = null;

    private KinectManager kinectManager = null;

    // Public bool to determine whether to track face model data or not
    public bool getFaceModelData = false;

    // Public Bool to determine whether to display face rectangle on the GUI
    public bool displayFaceRect = false;

    // Tolerance (in seconds) allowed to miss the tracked face before losing it
    public float faceTrackingTolerance = 0.25f;

    // Public Bool to determine whether the model mesh should be mirrored or not
    public bool mirroredModelMesh = true;

    // GUI Text to show messages.
    public GUIText debugText;

    // Tracked face rectangle
    //	private Rect faceRect;
    //	private bool bGotFaceRect;

    // primary sensor data structure
    private KinectInterop.SensorData sensorData = null;

    // Bool to keep track of whether face-tracking system has been initialized
    private bool isFacetrackingInitialized = false;

    // Keeps track of whether or not faces were updated to prevent reduntant updating each frame
    private bool facesUpdatedSuccessfully = false;

    // The single instance of FacetrackingManager
    private static FacetrackingManager instance;

    // returns the single FacetrackingManager instance
    public static FacetrackingManager Instance
    {
        get
        {
            return instance;
        }
    }

    // returns true if facetracking system was successfully initialized, false otherwise
    public bool IsFaceTrackingInitialized()
    {
        return isFacetrackingInitialized;
    }

    // returns the skeleton ID of the tracked user, or 0 if no user was associated with the face
    public long GetFaceTrackingID(int playerIndex)
    {
        return playerData[playerIndex].IsTrackingFace() ? playerData[playerIndex].primaryUserID : 0;
    }

    // returns true if the the face of the specified user is being tracked, false otherwise
    public bool IsTrackingFace(long userId)
    {
        if (sensorData != null && sensorData.sensorInterface != null)
        {
            return sensorData.sensorInterface.IsFaceTracked(userId);
        }


        return false;
    }

    public bool IsTrackingAnyFace()
    {
        foreach (PlayerFacetrackingData singlePlayerData in playerData)
            if (singlePlayerData.IsTrackingFace())
                return true;
        return false;
    }

    // returns the tracked head position
    public Vector3 GetHeadPosition(bool bMirroredMovement, int index)
    {
        Vector3 vHeadPos = playerData[index].bGotHeadPos ? 
            playerData[index].headPos : Vector3.zero;

        if (!bMirroredMovement)
        {
            vHeadPos.z = -vHeadPos.z;
        }

        return vHeadPos;
    }

    // returns the tracked head position for the specified user
    public Vector3 GetHeadPosition(long userId, bool bMirroredMovement)
    {
        Vector3 vHeadPos = Vector3.zero;
        bool bGotPosition = sensorData.sensorInterface.GetHeadPosition(userId, ref vHeadPos);

        if (bGotPosition)
        {
            if (!bMirroredMovement)
            {
                vHeadPos.z = -vHeadPos.z;
            }

            return vHeadPos;
        }

        return Vector3.zero;
    }

    // returns the tracked head rotation
    public Quaternion GetHeadRotation(bool bMirroredMovement, int index)
    {
        Vector3 rotAngles = playerData[index].bGotHeadRot ? 
            playerData[index].headRot.eulerAngles : Vector3.zero;

        if (bMirroredMovement)
        {
            rotAngles.x = -rotAngles.x;
            rotAngles.z = -rotAngles.z;
        }
        else
        {
            rotAngles.x = -rotAngles.x;
            rotAngles.y = -rotAngles.y;
        }

        return Quaternion.Euler(rotAngles);
    }

    // returns the tracked head rotation for the specified user
    public Quaternion GetHeadRotation(long userId, bool bMirroredMovement)
    {
        Quaternion vHeadRot = Quaternion.identity;
        bool bGotRotation = sensorData.sensorInterface.GetHeadRotation(userId, ref vHeadRot);

        if (bGotRotation)
        {
            Vector3 rotAngles = vHeadRot.eulerAngles;

            if (bMirroredMovement)
            {
                rotAngles.x = -rotAngles.x;
                rotAngles.z = -rotAngles.z;
            }
            else
            {
                rotAngles.x = rotAngles.x;
                rotAngles.y = rotAngles.y;
            }

            return Quaternion.Euler(rotAngles);
        }

        return Quaternion.identity;
    }

    // returns the tracked face rectangle for the specified user in color coordinates, or zero-rect if the user's face is not tracked
    public Rect GetFaceColorRect(long userId)
    {
        Rect faceRect = new Rect();
        sensorData.sensorInterface.GetFaceRect(userId, ref faceRect);

        return faceRect;
    }

    // returns true if there are valid anim units
    public bool IsGotAU(int index)
    {
        return playerData[index].bGotAU;
    }

    // returns the animation unit at given index, or 0 if the index is invalid
    public float GetAnimUnit(KinectInterop.FaceShapeAnimations faceAnimKey, int index)
    {
        if (playerData[index].dictAU.ContainsKey(faceAnimKey))
        {
            return playerData[index].dictAU[faceAnimKey];
        }

        return 0.0f;
    }

    // gets all animation units for the specified user. returns true if the user's face is tracked, false otherwise
    public bool GetUserAnimUnits(long userId, ref Dictionary<KinectInterop.FaceShapeAnimations, float> dictAnimUnits)
    {
        if (sensorData != null && sensorData.sensorInterface != null)
        {
            bool bGotIt = sensorData.sensorInterface.GetAnimUnits(userId, ref dictAnimUnits);
            return bGotIt;
        }

        return false;
    }

    // returns true if there are valid shape units
    public bool IsGotSU(int index)
    {
        return playerData[index].bGotSU;
    }

    // returns the shape unit at given index, or 0 if the index is invalid
    public float GetShapeUnit(KinectInterop.FaceShapeDeformations faceShapeKey, int index)
    {
        if (playerData[index].dictSU.ContainsKey(faceShapeKey))
        {
            return playerData[index].dictSU[faceShapeKey];
        }

        return 0.0f;
    }

    // gets all animation units for the specified user. returns true if the user's face is tracked, false otherwise
    public bool GetUserShapeUnits(long userId, ref Dictionary<KinectInterop.FaceShapeDeformations, float> dictShapeUnits)
    {
        if (sensorData != null && sensorData.sensorInterface != null)
        {
            bool bGotIt = sensorData.sensorInterface.GetShapeUnits(userId, ref dictShapeUnits);
            return bGotIt;
        }

        return false;
    }

    // returns the count of face model vertices
    public int GetFaceModelVertexCount(int index)
    {
        if (playerData[index].bGotModelVertices)
        {
            return playerData[index].avModelVertices.Length;
        }

        return 0;
    }

    // returns the face model vertices, if face model is available and index is in range; Vector3.zero otherwise
    public Vector3 GetFaceModelVertex(int index)
    {
        if (playerData[index].bGotModelVertices)
        {
            if (index >= 0 && index < playerData[index].avModelVertices.Length)
            {
                return playerData[index].avModelVertices[index];
            }
        }

        return Vector3.zero;
    }

    // returns the face model vertices, if face model is available; null otherwise
    public Vector3[] GetFaceModelVertices(int index)
    {
        if (playerData[index].bGotModelVertices)
        {
            return playerData[index].avModelVertices;
        }

        return null;
    }

    // gets all face model vertices for the specified user. returns true if the user's face is tracked, false otherwise
    public bool GetUserFaceVertices(long userId, ref Vector3[] avVertices)
    {
        if (sensorData != null && sensorData.sensorInterface != null)
        {
            bool bGotIt = sensorData.sensorInterface.GetFaceModelVertices(userId, ref avVertices);
            return bGotIt;
        }

        return false;
    }

    // returns the face model triangle indices, if face model is available; null otherwise
    public int[] GetFaceModelTriangleIndices(bool bMirroredModel)
    {
        if (sensorData != null && sensorData.sensorInterface != null)
        {
            int iNumTriangles = sensorData.sensorInterface.GetFaceModelTrianglesCount();

            if (iNumTriangles > 0)
            {
                int[] avModelTriangles = new int[iNumTriangles];
                bool bGotModelTriangles = sensorData.sensorInterface.GetFaceModelTriangles(bMirroredModel, ref avModelTriangles);

                if (bGotModelTriangles)
                {
                    return avModelTriangles;
                }
            }
        }

        return null;
    }

    public GameObject getFaceModelMesh(int index)
    {
        return playerData[index].faceModelMesh;
    }

    //----------------------------------- end of public functions --------------------------------------//
    //checks if faces were updated yet this frame. if not, updates facetracker for all faces
    bool updateAllFaces()
    {
        if (!facesUpdatedSuccessfully)
        {
            sensorData.sensorInterface.UpdateFaceTracking();
            return true;
        }
        return false;
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        //DontDestroyOnLoad(gameObject);

        playerData = new PlayerFacetrackingData[players];

        for (int i = 0; i < players; i++)
        {
            playerData[i] = new PlayerFacetrackingData();
            playerData[i].faceModelMesh = faceModelMesh[i];
            // playerData's faceModelMesh was assigned, so delete FacetrackingManager's to reduce memory usage
            faceModelMesh[i] = null; 
        }
        faceModelMesh = null;

        for (int i = 0; i < players; i++)
            playerData[i].getFaceModelData = getFaceModelData;
        
        try
        {
            // get sensor data
            kinectManager = KinectManager.Instance;
            if (kinectManager && kinectManager.IsInitialized())
            {
                sensorData = kinectManager.GetSensorData();
            }
            if (sensorData == null || sensorData.sensorInterface == null)
            {
                throw new Exception("Face tracking cannot be started, because KinectManager is missing or not initialized.");
            }

            if (debugText != null)
            {
                debugText.GetComponent<GUIText>().text = "Please, wait...";
            }

            // ensure the needed dlls are in place and face tracking is available for this interface
            bool bNeedRestart = false;
            if (sensorData.sensorInterface.IsFaceTrackingAvailable(ref bNeedRestart))
            {
                if (bNeedRestart)
                {
                    KinectInterop.RestartLevel(gameObject, "FM");
                    return;
                }
            }
            else
            {
                string sInterfaceName = sensorData.sensorInterface.GetType().Name;
                throw new Exception(sInterfaceName + ": Face tracking is not supported!");
            }

            // Initialize the face tracker
            if (!sensorData.sensorInterface.InitFaceTracking(getFaceModelData, displayFaceRect))
            {
                throw new Exception("Face tracking could not be initialized.");
            }

            isFacetrackingInitialized = true;

            if (debugText != null)
            {
                debugText.GetComponent<GUIText>().text = "Ready.";
            }
        }
        catch (DllNotFoundException ex)
        {
            Debug.LogError(ex.ToString());
            if (debugText != null)
                debugText.GetComponent<GUIText>().text = "Please check the Kinect and FT-Library installations.";
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            if (debugText != null)
                debugText.GetComponent<GUIText>().text = ex.Message;
        }
    }

    void OnDestroy()
    {
        if (isFacetrackingInitialized && sensorData != null && sensorData.sensorInterface != null)
        {
            // finish face tracking
            sensorData.sensorInterface.FinishFaceTracking();
        }
        //		// clean up
        //		Resources.UnloadUnusedAssets();
        //		GC.Collect();

        isFacetrackingInitialized = false;
        instance = null;
    }

    void Update()
    {
        if (isFacetrackingInitialized)
        {
            if (kinectManager == null)
                kinectManager = KinectManager.Instance;
            
            //update faces
            facesUpdatedSuccessfully = updateAllFaces();
            for (int i = 0; i < players; i++)
            {
                if (kinectManager && kinectManager.IsInitialized())
                    playerData[i].primaryUserID = kinectManager.GetUserIdByIndex(i);

                playerData[i].updatePlayer(facesUpdatedSuccessfully, sensorData);
            }
            facesUpdatedSuccessfully = false;
        }
    }

    void OnGUI()
    {
        if (isFacetrackingInitialized)
        {
            if (debugText != null)
            {
                if (playerData[0].isTrackingFace)
                {
                    debugText.GetComponent<GUIText>().text = 
                            "Tracking - skeletonID: " + playerData[0].primaryUserID;
                }
                else
                {
                    debugText.GetComponent<GUIText>().text = "Not tracking...";
                }
            }
        }
    }

    public Vector3 getMeshTransform(int index)
    {
        return playerData[index].getMeshTransform();
    }
}