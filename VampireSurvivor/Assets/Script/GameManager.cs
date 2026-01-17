using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider expSlider;
    public Text lvTXT;
    private int level = 1;
    public float expSpeed;

    [Header("EXP Info")]
    private float playerExp;
    private float maxExp = 20f;
    private float expPer; //정규화된 Exp

    [Header("Skill")]
    public List<Transform> skill = new List<Transform>();
    public List<Transform> randSkill = new List<Transform>();

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

        if(Input.GetKey(KeyCode.R))
        {
            RandomSkillOn();
        }
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
                        maxExp = maxExp * 2.0f;
                        expSlider.value = 0;
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
    }
}
