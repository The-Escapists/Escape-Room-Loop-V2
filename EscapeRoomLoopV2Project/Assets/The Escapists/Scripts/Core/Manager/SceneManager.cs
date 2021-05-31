using Sirenix.OdinInspector;
using UnityEngine;

namespace TheEscapists.Core.Manager
{
    public class SceneManager : MonoBehaviour
    {

        [Button("Update Scene Cache")]
        private void UpdateSceneChache()
        {
            // TagsCreator.UpdateSceneCacheUnity();
        }

        //ublic UnityScenes.eUnityScenes startScene;

        public void LoadGameScene()
        {
            //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(startScene.ToString());
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}