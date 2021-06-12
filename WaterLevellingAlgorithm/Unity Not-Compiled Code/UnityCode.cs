
// Unity code which I've created for visualization of the output
// Code is compilable, but only with Unity .dll's

using System;
using System.Runtime.InteropServices;
using Assets.Alg;
using UnityEngine;

namespace Assets
{
    public class PoziomWody : MonoBehaviour
    {
        public GameObject cuboidCisternModel;
        public GameObject cylinderCisternModel;
        public GameObject sphereCisternModel;
        public GameObject coneCisternModel;

        public GameObject cuboidWaterModel;
        public GameObject cylinderWaterModel;
        public GameObject sphereWaterModel;
        public GameObject coneWaterModel;

        public GameObject textObject;

        public Material waterMaterial;
        public Shader waterShader;

        private Cistern[] _cysInfo;
        private Vector3 _upcomingSpawnPosition = new Vector3(0, 0, 0);
        private Vector3 _upcomingWaterSpawnPosition = new Vector3(0, 0, 0);
        private readonly Quaternion _rotationToSpawn = new Quaternion(0, 0, 0, 0);
        private double _level;

        Vector3 GetCisternDimensions(Cistern cysterna)
        {
            Vector3 delta = new Vector3();
            switch (cysterna)
            {
                case ConeCistern coneCistern:
                    delta.x = ((ConeCistern)cysterna).Radius * 2;
                    delta.y = ((ConeCistern)cysterna).Height;
                    delta.z = ((ConeCistern)cysterna).Radius * 2;
                    break;

                case CuboidCistern cuboidCistern:
                    delta.x = ((CuboidCistern)cysterna).Width;
                    delta.y = ((CuboidCistern)cysterna).Height;
                    delta.z = ((CuboidCistern)cysterna).Depth;
                    break;

                case CylinderCistern cylinderCistern:
                    delta.x = ((CylinderCistern)cysterna).Radius * 2;
                    delta.y = ((CylinderCistern)cysterna).Height / 2;
                    delta.z = ((CylinderCistern)cysterna).Radius * 2;
                    break;

                case SphereCistern sphereCistern:
                    delta.x = ((SphereCistern)cysterna).Radius * 2;
                    delta.y = ((SphereCistern)cysterna).Radius * 2;
                    delta.z = ((SphereCistern)cysterna).Radius * 2;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(cysterna));
            }

            return delta;
        }

        void GetCisternModelGameObject(Cistern cysterna, out GameObject cistern, out GameObject water)
        {
            switch (cysterna)
            {
                case ConeCistern coneCistern:
                    cistern = coneCisternModel;
                    water = coneWaterModel;
                    break;

                case CuboidCistern cuboidCistern:
                    cistern = cuboidCisternModel;
                    water = cuboidWaterModel;
                    break;

                case CylinderCistern cylinderCistern:
                    cistern = cylinderCisternModel;
                    water = cylinderWaterModel;
                    break;

                case SphereCistern sphereCistern:
                    cistern = sphereCisternModel;
                    water = sphereWaterModel;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(cysterna));
            }
        }

        void LiquifyObject(ref GameObject go, float fillValue)
        {

            Material mat = new Material(waterShader);
            mat.SetFloat("_Fill", fillValue);
            go.GetComponent<MeshRenderer>().material = mat;
        }

        // Start is called before the first frame update
        void Start()
        {

            string line = "4 cub 9 4 2 3 sph 7 5 cyl 10 4 2 cyl 7 2 3 600"; //input line
            DataModeler dr = new DataModeler();
            _level = dr.ModelAssignAndLevellingFromString(line);

            if (_level < 0) //If Overflowed
            {
                Instantiate(textObject); //Spawn 3D Text object which tells "OVERFLOW!"
                return;
            }

            _cysInfo = dr.GetCurrentModelCisterns();
            float fillValue = 0;
            

            foreach (var cistern in _cysInfo) //Spawning Cisterns
            {
                Vector3 Dimensions = GetCisternDimensions(cistern);
                GetCisternModelGameObject(cistern, out GameObject cisternModel, out GameObject waterModel);

                _upcomingSpawnPosition.x += Dimensions.x / 2; //Making a space for myself
                _upcomingWaterSpawnPosition.x += Dimensions.x / 2;

                double objetoscWodyWCysternie = cistern.CalculateVolumeFromLevel(_level);
                float percentageOfFullfillment = (float)(objetoscWodyWCysternie / cistern.Volume());

                fillValue = cistern is CylinderCistern
                    ? -Dimensions.y + (Dimensions.y * 2 * percentageOfFullfillment) //Adjusting fillValue to Unity Shader standard 
                    : - Dimensions.y + (Dimensions.y * percentageOfFullfillment);   //Range of fullfillment is <-Height/2;Height/2>, not <0;1>

                Vector3 cisScale = Dimensions;
                float scale = (float)1.05;
                cisScale.Scale(new Vector3(scale, scale, scale)); //Adjusting the thickness of glass water tank
                cisternModel.transform.localScale = cisScale;
                waterModel.transform.localScale = Dimensions;

                _upcomingSpawnPosition.y = cistern.BaseLevel + cisternModel.transform.localScale.y;     //Adjusting spawn position to Unity standard 
                _upcomingWaterSpawnPosition.y = cistern.BaseLevel + waterModel.transform.localScale.y;  // When object height is one, it needs to be placed iwth y=1, if we want its bottom edge to be at the level of zero.


                Instantiate(cisternModel, _upcomingSpawnPosition, _rotationToSpawn);    //Deploying cistern in-game object
                LiquifyObject(ref waterModel, fillValue); ,                             //Updating water object material & shader, setting fullfillment
                Instantiate(waterModel, _upcomingWaterSpawnPosition, _rotationToSpawn); //Deploying water in-game object 

                _upcomingSpawnPosition.x += Dimensions.x / 2 + (float)1.5;      //..also making a space for upcoming cistern
                _upcomingWaterSpawnPosition.x += Dimensions.x / 2 + (float)1.5;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Plans : Creating the Cisterns SQL database, deploying new cisterns at runtime in Unity with an app created with ASP.NET Core
        }



    }
}

    
   