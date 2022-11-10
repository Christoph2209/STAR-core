using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class NextSceneOnVideoEnd : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<VideoPlayer>().loopPointReached += (UnityEngine.Video.VideoPlayer vp) => 
        {
            SceneManager.LoadScene("StartMenu");
        };

    }


}
