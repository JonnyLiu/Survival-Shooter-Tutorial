using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;//剪辑


    Animator anim;
    AudioSource enemyAudio;//音频
    ParticleSystem hitParticles;//粒子系统
    CapsuleCollider capsuleCollider;//碰撞体
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();//获取动画控制器
        enemyAudio = GetComponent <AudioSource> ();//获取音频
        hitParticles = GetComponentInChildren <ParticleSystem> ();//获取音频
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)//判断角色死亡
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);//角色受损 播放音频 减血
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;//粒子特效的位置为碰撞点位置
        hitParticles.Play();//粒子播放

        if(currentHealth <= 0)//如果生命值小于0死亡
        {
            Death ();
        }
    }


    void Death ()//死亡函数
    {
        isDead = true;//死亡为真

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");//激活死亡参数

        enemyAudio.clip = deathClip;//播放死亡动画片段
        enemyAudio.Play ();
    }


    public void StartSinking ()//死亡的僵尸销毁
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;//关闭寻路组件
        GetComponent <Rigidbody> ().isKinematic = true;//钢铁动力学开启
        isSinking = true;//设置isSinking为真
        ScoreManager.score += scoreValue; //更新ScoreManager文本计算分数
        Destroy (gameObject, 2f);//销毁 2F后
    }
}
