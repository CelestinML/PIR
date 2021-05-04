using UnityEngine;

public class InfosVaisseauTraining : MonoBehaviour
{
    //Référence vers le gestionnaire des animations du vaisseau
    private Animator animator;

    //Nombre de vies restantes avant d'enclencher la fin du jeu
    public int points_de_vie = 3;

    //Infos pour le calcul de l'invincibilité (modifiable pour ajuster la difficulté)
    private bool invincible = false;
    public float temps_invincibilite = 1;
    private float invincible_time = 0;

    //Détermine la vitesse de clignotement
    private float blink_period = 0.1f;
    private float blink_time = 0;

    //Infos pour le calcul du score
    private float points_per_second = 10;
    private float score = 0;
    private bool stop_score = false;

    //Infos pour booster la difficulté en fonction du score
    // private bool period_boost_block = false;
    // private List<int> boost_moments;
    // private bool max_speed_boost_block = false;
    // private List<int> max_speed_boost_moments;

    private ShipSpawnerTraining spawnerManagement;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spawnerManagement = Camera.main.GetComponent<ShipSpawnerTraining>();
    }

    private void FixedUpdate()
    {
        //On calcule et on affiche le score
        if (!stop_score)
            score += Time.fixedDeltaTime * points_per_second;

        //On détermine si le vaisseau est invincible ou non
        if (invincible)
        {
            invincible_time += Time.fixedDeltaTime;
            blink_time += Time.fixedDeltaTime;
            if (blink_time > blink_period)
            {
                if (gameObject.GetComponent<Renderer>().enabled)
                    gameObject.GetComponent<Renderer>().enabled = false;
                else
                    gameObject.GetComponent<Renderer>().enabled = true;
                blink_time = 0;
            }
            if (invincible_time > temps_invincibilite)
            {
                invincible = false;
                gameObject.GetComponent<Renderer>().enabled = true;
                invincible_time = 0;
                blink_time = 0;
            }
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (!invincible)
        {
            points_de_vie -= damage;

            if (points_de_vie == 0)
            {
                HandleVaisseauDeath();
            }
            else
            {
                invincible = true;
            }
        }
    }

    private void DisableColliders()
    {
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders)
            collider.enabled = false;
    }


    public void HandleVaisseauDeath()
    {
        spawnerManagement.UpdateChildScore(gameObject, score);
        gameObject.SetActive(false);
    }
}
