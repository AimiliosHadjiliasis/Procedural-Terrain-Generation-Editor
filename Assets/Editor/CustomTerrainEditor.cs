﻿using UnityEngine;
using UnityEditor;
using EditorGUITable;

//links editor code and Custom terrain script
//so it knows when we add anyting to an object in the hierarchy when we are looking in the inspector
//then come back to this code and get all the details on how it should be displayed
[CustomEditor(typeof(CustomTerrain))]   
[CanEditMultipleObjects]    //can apply to multiple objects
public class CustomTerrainEditor : Editor
{
    /**************************************************/
    //                  Properties:
    /**************************************************/
    //References (Link with other script):
    SerializedProperty randomHeightRange;
    SerializedProperty heightMapScale;
    SerializedProperty heightMapImage;
    SerializedProperty perlinXScale;
    SerializedProperty perlinYScale;
    SerializedProperty perlinOffsetX;
    SerializedProperty perlinOffsetY;
    SerializedProperty perlinOctaves;
    SerializedProperty perlinPersistance;
    SerializedProperty perlinHeightScale;
    SerializedProperty perlinfBMXScale;
    SerializedProperty perlinfBMYScale;
    SerializedProperty perlinfBMOffsetX;
    SerializedProperty perlinfBMOffsetY;
    SerializedProperty resetTerrain;

    GUITableState multiplePerlinParametersTable;
    SerializedProperty multiplePerlinParameters;

    SerializedProperty voronoiFallOff;
    SerializedProperty voronoiDropOff;
    SerializedProperty voronoiMinHeight;
    SerializedProperty voronoiMaxHeight;
    SerializedProperty voronoiPeaks;
    SerializedProperty voronoiType;

    SerializedProperty MPDheightMin;
    SerializedProperty MPDheightMax;
    SerializedProperty MPDheightDampenerPower;
    SerializedProperty MPDroughness;

    SerializedProperty smoothAmount;

    GUITableState splatMapTable;
    SerializedProperty splatHeights;

    /**************************************************/
    //                  Foldouts:
    /**************************************************/
    bool showRandom = false;
    bool showLoadHeights = false;
    bool showSimplePerlinNoise = false;
    bool showfBMPerlinNoise = false;
    bool showMultiplePerlin = false;
    bool showVoronoiIcyPeaks = false;
    bool showVoronoiSingleMountainPeak = false;
    bool showVoronoiMultipleMountainPeak = false;
    bool showMPD = false;
    bool showSmooth = false;
    bool showSplatMaps = false;

    //method that allouw us to setup anything when we are setting something new into our editor
    //it disapbles and re-enables everything everytime we make a change without pressing play (same as awake method)
    void OnEnable()
    {
        //Syncronised with the custom terrain script:
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        heightMapScale = serializedObject.FindProperty("heightMapScale");
        heightMapImage = serializedObject.FindProperty("heightMapImage");
        perlinXScale = serializedObject.FindProperty("perlinXScale");
        perlinYScale = serializedObject.FindProperty("perlinYScale");
        perlinOffsetX = serializedObject.FindProperty("perlinOffsetX");
        perlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
        perlinOctaves = serializedObject.FindProperty("perlinOctaves");
        perlinPersistance = serializedObject.FindProperty("perlinPersistance");
        perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");
        perlinfBMXScale = serializedObject.FindProperty("perlinfBMXScale");
        perlinfBMYScale = serializedObject.FindProperty("perlinfBMYScale");
        perlinfBMOffsetX = serializedObject.FindProperty("perlinfBMOffsetX");
        perlinfBMOffsetY = serializedObject.FindProperty("perlinfBMOffsetY");
        resetTerrain = serializedObject.FindProperty("resetTerrain");

        multiplePerlinParametersTable = new GUITableState("multiplePerlinParametersTable");
        multiplePerlinParameters = serializedObject.FindProperty("multiplePerlinParameters");

        voronoiFallOff = serializedObject.FindProperty("voronoiFallOff");
        voronoiDropOff = serializedObject.FindProperty("voronoiDropOff");
        voronoiMinHeight = serializedObject.FindProperty("voronoiMinHeight");
        voronoiMaxHeight = serializedObject.FindProperty("voronoiMaxHeight");
        voronoiPeaks = serializedObject.FindProperty("voronoiPeaks");
        voronoiType = serializedObject.FindProperty("voronoiType");

        MPDheightMin = serializedObject.FindProperty("MPDheightMin");
        MPDheightMax = serializedObject.FindProperty("MPDheightMax");
        MPDheightDampenerPower = serializedObject.FindProperty("MPDheightDampenerPower");
        MPDroughness = serializedObject.FindProperty("MPDroughness");
        smoothAmount = serializedObject.FindProperty("smoothAmount");

        splatMapTable = new GUITableState("splatMapTable");
        splatHeights = serializedObject.FindProperty("splatHeights");
    }

    //Method that overrides the GUI 
    public override void OnInspectorGUI()
    {
        /*******************************************************************/
        //UPDATES all serialized properties between this 
        //script and the Custom Terrain script
        serializedObject.Update();
        
        //Link with the custom terrain script
        CustomTerrain terrain = (CustomTerrain)target;

        //Reset Box:
        EditorGUILayout.PropertyField(resetTerrain);

        /*************************************************************/
        //                      Random Heights:
        /*************************************************************/
        showRandom = EditorGUILayout.Foldout(showRandom, "Random"); //set true false to foldout
        //show if foldout is true
        if (showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  //Generate slider
            GUILayout.Label("Set Heights between Random Values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHeightRange);

            //Create button that generates random terrain:
            if (GUILayout.Button("Random Heights"))
            {
                terrain.RandomTerrain();
            }
        }

        /*************************************************************/
        //                  Load Heights from Image:
        /*************************************************************/
        showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Load Heights");
        if (showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights From Texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(heightMapImage);
            EditorGUILayout.PropertyField(heightMapScale);

            //Create button that load the texture from an image
            if (GUILayout.Button("Load Texture"))
            {
                terrain.LoadTexture();
            }
        }


        /*************************************************************/
        //     Load Heigths based on Simple Perlin Noise 
        /*************************************************************/
        showSimplePerlinNoise = EditorGUILayout.Foldout(showSimplePerlinNoise, "Simple Perlin Noise");
        if (showSimplePerlinNoise)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Perlin Noise", EditorStyles.boldLabel);

            //Add a slider as for scale X and Y values:
            EditorGUILayout.Slider(perlinXScale, 0, 1, new GUIContent("X Scale"));
            EditorGUILayout.Slider(perlinYScale, 0, 1, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinOffsetX, 0, 10000, new GUIContent("Offset X"));
            EditorGUILayout.IntSlider(perlinOffsetY, 0, 10000, new GUIContent("Offset Y"));

            //Create a button that generates a height map based on Perlin noise
            if (GUILayout.Button("Simple Perlin Noise"))
            {
                terrain.PerlinNoise();
            }
        }

        /****************************************************************/
        //  Load Heigths based on Simple Perlin Noise that uses fBM 
        /****************************************************************/
        showfBMPerlinNoise = EditorGUILayout.Foldout(showfBMPerlinNoise, "fBM Perlin Noise");
        if (showfBMPerlinNoise)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("fBM Perlin Noise", EditorStyles.boldLabel);

            //Add a slider as for scale X and Y values:
            EditorGUILayout.Slider(perlinfBMXScale, 0, 1, new GUIContent("X Scale"));
            EditorGUILayout.Slider(perlinfBMYScale, 0, 1, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinfBMOffsetX, 0, 10000, new GUIContent("Offset X"));
            EditorGUILayout.IntSlider(perlinfBMOffsetY, 0, 10000, new GUIContent("Offset Y"));
            EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
            EditorGUILayout.Slider(perlinPersistance, 0.1f, 10, new GUIContent("Persistance"));
            EditorGUILayout.Slider(perlinHeightScale, 0, 1, new GUIContent("Height Scale"));

            //Create a button that generates a height map based on fBM Perlin noise
            if (GUILayout.Button("fBM Perlin Noise"))
            {
                terrain.fBMPerlinNoise();
            }
        }

        /****************************************************************/
        //  Load Heigths with the use of multile perlin noise: 
        /****************************************************************/
        showMultiplePerlin = EditorGUILayout.Foldout(showMultiplePerlin, "Multiple Perlin Noise");
        if (showMultiplePerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Mulitple Perlin Noise", EditorStyles.boldLabel);

            //Generate a table with the parameters:
            multiplePerlinParametersTable = GUITableLayout.DrawTable(multiplePerlinParametersTable, //Table
                                                            multiplePerlinParameters);  //Parameters

            //Leave a space so we can see the slider:
            GUILayout.Space(30);

            //Begin horizontal block to add the +/- buttons in the same line
            EditorGUILayout.BeginHorizontal();

            //+ button (Add)
            if (GUILayout.Button("+"))
            {
                terrain.AddNewPerlin();
            }

            //- button (Remove)
            if (GUILayout.Button("-"))
            {
                terrain.RemovePerlin();
            }

            //End horizontal block to add the +/- buttons in the same line
            EditorGUILayout.EndHorizontal();

            //Button that apply the changes to the terrain
            if (GUILayout.Button("Apply Multiple Perlin"))
            {
                terrain.MultiplePerlinNoiseTerrain();
            }
        }

        /****************************************************************/
        // Load Heigths with the use of voronoi tessalation (Icy spikes)
        /****************************************************************/
        showVoronoiIcyPeaks = EditorGUILayout.Foldout(showVoronoiIcyPeaks, "Voronoi Icy Peaks");
        if (showVoronoiIcyPeaks)
        {
            if (GUILayout.Button("Generate Icy Peaks (Voronoi)"))
            {
                terrain.VoronoiIcyPeaks();
            }
        }


        /*****************************************************************************/
        //  Load Heigths with the use of voronoi tessalation (Multiple Mountain Peak)
        /*****************************************************************************/
        showVoronoiMultipleMountainPeak = EditorGUILayout.Foldout(showVoronoiMultipleMountainPeak, "Voronoi Multiple Mountain Peaks");
        if (showVoronoiMultipleMountainPeak)
        {
            EditorGUILayout.IntSlider(voronoiPeaks, 1, 10, new GUIContent("Peak Count"));
            EditorGUILayout.Slider(voronoiFallOff, 0, 10, new GUIContent("Falloff"));
            EditorGUILayout.Slider(voronoiDropOff, 0, 10, new GUIContent("Dropoff"));
            EditorGUILayout.Slider(voronoiMinHeight, 0, 1, new GUIContent("Min Height"));
            EditorGUILayout.Slider(voronoiMaxHeight, 0, 1, new GUIContent("Max Height"));
            EditorGUILayout.PropertyField(voronoiType);

            if (GUILayout.Button("Generate Multiple Mountain Peaks (Voronoi)"))
            {
                terrain.VoronoiMultipleMountainPeak();
            }
        }

        /****************************************************************/
        //      Load height based on mid point displacement method:
        /****************************************************************/
        showMPD = EditorGUILayout.Foldout(showMPD, "Midpoint Displacement");
        if (showMPD)
        {
            EditorGUILayout.PropertyField(MPDheightMin);
            EditorGUILayout.PropertyField(MPDheightMax);
            EditorGUILayout.PropertyField(MPDheightDampenerPower);
            EditorGUILayout.PropertyField(MPDroughness);
            if (GUILayout.Button("MPD"))
            {
                terrain.MidPointDisplacement();
            }
        }

        /****************************************************************/
        //                  Show smoothing in the editor:
        /****************************************************************/
        showSmooth = EditorGUILayout.Foldout(showSmooth, "Smooth Terrain");
        if (showSmooth)
        {
            EditorGUILayout.IntSlider(smoothAmount, 1, 10, new GUIContent("smoothAmount"));
            if (GUILayout.Button("Smooth"))
            {
                terrain.Smooth();
            }
        }

        /****************************************************************/
        //              Show splat table in the editior:
        /****************************************************************/
        showSplatMaps = EditorGUILayout.Foldout(showSplatMaps, "Splat Maps");
        if (showSplatMaps)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Splat Mpas", EditorStyles.boldLabel);
            splatMapTable = GUITableLayout.DrawTable(splatMapTable, splatHeights);

            //Leave a space so we can see the slider:
            GUILayout.Space(30);

            //Begin horizontal block to add the +/- buttons in the same line
            EditorGUILayout.BeginHorizontal();

            //+ button (Add)
            if (GUILayout.Button("+"))
            {
                terrain.AddNewSplatHeight();
            }

            //- button (Remove)
            if (GUILayout.Button("-"))
            {
                terrain.RemoveSplatHeight();
            }

            //End horizontal block to add the +/- buttons in the same line
            EditorGUILayout.EndHorizontal();

            //Button that apply the changes to the terrain
            if (GUILayout.Button("Apply Splat Maps"))
            {
                terrain.SplatMaps();
            }
        }

        /************************************************************/
        // Reset Button that resets the terrain back to normal(flat)
        /************************************************************/
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Reset Terrain"))
        {
            terrain.ResetTerrain();
        }

        //Apply all the changes:
        serializedObject.ApplyModifiedProperties();
        /*******************************************************************/
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
