using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public Player player;

    public void Awake ()
    {
        SceneManager.LoadScene("PlayerScene", LoadSceneMode.Additive);
    }

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void GoToDuel()
    {
        SceneManager.LoadScene("Duel", LoadSceneMode.Single);
    }
}
