using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public Player player;

    public Slider expSlider;
    public Text lvTXT;
    public int level = 1;
    public float expSpeed;

    [Header("EXP Info")]
    private float playerExp;
    private float maxExp = 20f;
    private float expPer; //정규화된 Exp

    [Header("Skill")]
    public List<Transform> skill = new List<Transform>();
    public List<Transform> randSkill = new List<Transform>();
    public Image skillPanel;
    public Transform offPanel;
    public Transform onBoard;
    public float skillHp;
    public float skillSpeed;
    public float skillInterval;
    public float skillDamage;
    public GameObject specialAttack;

    public enum ExpState
    {
        None,
        ExpUp
    }
    public ExpState expState = ExpState.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerExp();
    }

    void PlayerExp()
    {
        switch (expState)
        {
            case ExpState.ExpUp:
                {
                    expSlider.value = Mathf.MoveTowards(expSlider.value, expPer, expSpeed * Time.deltaTime);
                    if (expSlider.value == 1.0f)
                    {
                        level++;
                        lvTXT.text = level.ToString();
                        playerExp = 0;
                        maxExp = level * 20.0f;
                        expSlider.value = 0;
                        RandomSkillOn();
                        Time.timeScale = 0f; //게임 일시 정지

                        expState = ExpState.None;
                    }
                    else
                    {
                        if (expSlider.value == expPer)
                        {
                            expState = ExpState.None;
                        }
                    }
                    break;
                }
        }
    }

    public void PlayerExpUp(float exp)
    {
        playerExp += exp;
        expPer = playerExp / maxExp;
        expState = ExpState.ExpUp;
    }

    void RandomSkillOn()
    {
        int rand = Random.Range(0, skill.Count);

        while (randSkill.Count < 3)
        {
            if (!randSkill.Contains(skill[rand]))
            {
                randSkill.Add(skill[rand]);
            }
            else
            {
                rand = Random.Range(0, skill.Count);
            }
        }

        skillPanel.enabled = true;
        
        for (int i = 0; i < randSkill.Count; i++)
        {
            randSkill[i].transform.parent = onBoard; //랜덤 스킬 부모를 온보드로 지정
        }
    }

    public void PlayerHpUp()
    {
        player.PlayerHpUp(skillHp);
        CloseSkillPanel();
    }
    public void BulletUp()
    {
        player.BulletUp(1);
        CloseSkillPanel();
    }
    public void SpeedUp()
    {
        player.SpeedUp(skillSpeed);
        CloseSkillPanel();
    }
    public void IntervalUp()
    {
        player.IntervalUp(skillInterval);
        CloseSkillPanel();
    }
    public void DamageUp()
    {
        player.DamageUp(skillDamage);
        CloseSkillPanel();
    }
    public void Special()
    {
        GameObject specialAtk = Instantiate(specialAttack, player.transform.position, Quaternion.identity);
        Destroy(specialAtk, 5f);

        CloseSkillPanel();
    }

    void CloseSkillPanel()
    {
        skillPanel.enabled = false;
        
        for (int i = 0; i < skill.Count; i++)
        {
            skill[i].transform.parent = offPanel; //모든 스킬의 부모를 오프패널로 지정
        }

        Time.timeScale = 1f;
    }
}
