using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private PrintDialLog dialLog;
    [SerializeField] private Animation diceAnim;
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private GameObject buffObjArr;
    [SerializeField] private ParticleSystem[] buffs;
    [SerializeField] private Animator shopAnim;
    [SerializeField] private string titleSceneName;
    
    private int _curNum;
    
    private void Awake()
    {
        dialLog.OnEndPrintText += HandleNextPrintTutorial;
        enemyAnim.transform.position = new Vector3(-15, 0, 0);
        
    }

    private void HandleNextPrintTutorial()
    {
        SelectTutorial(++_curNum);
    }

    private void SelectTutorial(int num)
    {
        switch (num)
        {
             case 1:
                 FirstTutorial();
                 break;
             case 2:
                 SecondTutorial();
                 break;
             case 3:
                 ThirdTutorial();
                 break;
             case 4:
                 FourthTutorial();
                 break;
             case 6:
                 //타이틀 씬으로 이동
                 SceneControlManager.FadeOut(()=>SceneManager.LoadScene(titleSceneName)); 
                 break;
        }
    }

    private void FirstTutorial()
    {
        diceAnim.Play();
    }

    private void SecondTutorial()
    {
        MoveNextTarget(diceAnim.gameObject, enemyAnim, "Damaged");
    }

    private void ThirdTutorial()
    {
        MoveNextTarget(enemyAnim.gameObject);

        buffObjArr.transform.DOMoveX(0, 0.7f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            buffs.ToList().ForEach(item =>
            {
                item.gameObject.SetActive(true);
                item.Play();
            });
        });
    }

    private void FourthTutorial()
    {
        buffs.ToList().ForEach(item =>
        {
            item.Stop();
        });
        MoveNextTarget(buffObjArr, shopAnim, "ShopMove");
    }
    
    private void MoveNextTarget(GameObject currnet, Animator animator = null, string animName = null)
    {
        
        currnet.transform.DOMoveX(15, 0.7f).SetEase(Ease.InOutBack).
            OnComplete(() =>
            {
                animator?.gameObject.transform.DOMoveX(0, 0.7f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    animator.Play(animName);
                });
            });
    }
}
