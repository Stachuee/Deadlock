using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class FacilityController : MonoBehaviour
{
    public static FacilityController facilityController;

    [SerializeField]
    public List<Rooms> allRooms;


    private void Awake()
    {
        if (facilityController == null) facilityController = this;
        else Debug.LogError("Two facility managers in one scene");
    }

    //Get all rooms and doors and save them into array
    public void ReimportRooms()
    {
        allRooms = FindObjectsOfType<Rooms>().ToList();
        //allRooms.ForEach(x => x.roomGUID = System.Guid.NewGuid().ToString());
    }

    int margin = 2;
    #region EditorScripts
    public void GetMapTexture()
    {
        MapSegment[] segments = FindObjectsOfType<MapSegment>();
        Texture2D[] mapTextures = new Texture2D[segments.Length];
        
        for(int i = 0; i < mapTextures.Length; i++)
        {
            mapTextures[i] = new Texture2D(segments[i].size.x*2 + margin, segments[i].size.y*2 + margin, TextureFormat.RGB24, false);
            for (int y = 0; y < segments[i].size.y * 2 + margin; y++)
            {
                for (int x = 0; x < segments[i].size.x * 2 + margin; x++)
                {
                    mapTextures[i].SetPixel(x, y, Color.black);
                }
            }
        }





        for (int i = 0; i < segments.Length; i++)
        {
            List<Rooms> allRoomsInSegment = allRooms.FindAll(x => Mathf.Abs(segments[i].gameObject.transform.position.x - x.transform.position.x) < segments[i].size.x && Mathf.Abs(segments[i].gameObject.transform.position.y - x.transform.position.y) < segments[i].size.y);

            Vector2Int bottomCorner = new Vector2Int(Mathf.FloorToInt(segments[i].transform.position.x) - segments[i].size.x, Mathf.FloorToInt(segments[i].transform.position.y) - segments[i].size.y);
            allRoomsInSegment.ForEach(room =>
            {
                Vector2Int start = new Vector2Int(Mathf.FloorToInt(room.transform.position.x - room.RoomSize.x) + margin, Mathf.FloorToInt(room.transform.position.y - room.RoomSize.y) + margin) - bottomCorner;
                for (int x = start.x; x < start.x + room.RoomSize.x * 2; x++)
                {
                    mapTextures[i].SetPixel(x, start.y, Color.green);
                    mapTextures[i].SetPixel(x, start.y + room.RoomSize.y * 2 - 1, Color.green);
                }

                for (int y = start.y; y < start.y + room.RoomSize.y * 2; y++)
                {
                    mapTextures[i].SetPixel(start.x, y, Color.green);
                    mapTextures[i].SetPixel(start.x + room.RoomSize.x * 2 - 1, y, Color.green);
                }

                //for(int doorIndex = 0; doorIndex < room.Doors.Length; doorIndex++)
                //{
                //    Vector2Int doorStart = new Vector2Int(Mathf.FloorToInt(room.transform.position.x + room.Doors[doorIndex].doorPosition.x) + margin, Mathf.FloorToInt(room.transform.position.y + room.Doors[doorIndex].doorPosition.y) + margin) - bottomCorner;
                //    mapTextures[i].SetPixel(doorStart.x, doorStart.y, Color.red);
                //}
                
            });
           
        }



        for (int i = 0; i < mapTextures.Length; i++)
        {
            byte[] bytes = mapTextures[i].EncodeToPNG();
            var dirPath = Application.dataPath + "/../Assets/Maps/";

            Debug.Log(dirPath); 
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + segments[i].sectorName + ".png", bytes);
            }

    }   
            
    private void OnDrawGizmos() 
    {

    }
    #endregion
}
