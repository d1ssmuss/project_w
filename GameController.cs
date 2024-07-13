using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;


[System.Serializable] // ?

public class Question
{
    public string Text; // сам вопрос
    public string[] Answers; // список строк с ответами
    [Range(0, 3)]
    public byte CorrectIndex; // правильный ответ
}
public class GameController : MonoBehaviour
{
    public TMP_Text _questionText;

    [Header("Answers")]
    public Button[] _answerButtons;
    public TMP_Text[] _answerButtonsText;

    [Header("Tips")]
    public Button _tip50; // 50:50
    public Button _tipCall; // звонок другу
    public Button _tiePeople; // Помощь зала
    public Button _tiex2; // право на ошибку


    [Header("Questions")]
    public Question[] _questions;


    [Header("Components")]
    public SpriteRenderer label_question; // синяя плашка, включающая в себя вопрос и ответ
    public TMP_Text Answ_a;
    public TMP_Text Answ_b;
    public TMP_Text Answ_c;
    public TMP_Text Answ_d;
    public Image sum_bar;
    public Text amount_bar;
    public Sound_game sound_game;
    public use_x2 lifeline_x2;

    public bool flashok = true;

    // Суммы
    // Создаю переменную для суммы
    public string Non_negotiable_sum = "0"; 


    /*
     * Переменные
     */
    private string[] total_prize = { // массив используется для плашки суммы
        "500",
        "1000",
        "2000",
        "3000",
        "5000",
        "10000",
        "15000",
        "25000",
        "50000",
        "100000",
        "200000",
        "400000",
        "800000",
        "1500000",
        "3000000"
    };


    [Header("Test")] // для тестирования системы
    [SerializeField] private byte _currentIndex = 0; // текущий индекс  (индекс текущего вопроса?)

    private void Awake()
    {
        var length = _answerButtons.Length;
        _answerButtonsText = new TMP_Text[_answerButtons.Length];
        for (var i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtonsText[i] = _answerButtons[i].GetComponentInChildren<TMP_Text>();
        }
    }


    private void SetQuestion()
    {
        var currentQuestion = _questions[_currentIndex];
        _questionText.text = _questions[_currentIndex].Text;
        for (var i = 0; i < _answerButtons.Length; i++)
        {
            var text = currentQuestion.Answers[i];
            switch (i)
            {
                case 0:
                    text = $"{text}";
                    break;
                case 1:
                    text = $"{text}";
                    break;
                case 2:
                    text = $"{text}";
                    break;
                case 3:
                    text = $"{text}";
                    break;
            }
            _answerButtonsText[i].text = text;
            //Debug.Log("Setq");
            _answerButtons[i].gameObject.SetActive(true);
            // поставить заглушку для плашки суммы
        }
    }



    private void Begin_Game()
    {
        _questionText.enabled = false;
        label_question.enabled = false;
        Answ_a.enabled = false;
        Answ_b.enabled = false;
        Answ_c.enabled = false;
        Answ_d.enabled = false;
        /*for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].enabled = true;
        }
        */
    }
    private void EndGame()
    {
        Debug.Log("Игра окончена");
    }


    public byte OnButtonClick(byte index)
    {
        int count_question = sound_game.count_q;
        Answ_a.color = Color.black;
        var correctIndex = _questions[_currentIndex].CorrectIndex;
        AudioSource final_otvet = sound_game.sound_final_answer[count_question];
        final_otvet.Play();
        Debug.Log("Текущий индекс: " + index + " " + "Индекс правильного ответа:" +  correctIndex);
        /*switch (index)
        {
            case 0:
                Answ_a.color = Color.black;
                break;
            case 1:
                Answ_b.color = Color.black;
                break;
            case 2:
                Answ_c.color = Color.black;
                break;
            case 3:
                Answ_d.color = Color.black;
                break;
        }
        */
        if ((index == correctIndex))
        {
            flashok = false;
            _currentIndex++;
            if (_currentIndex >= _questions.Length)
            {
                EndGame();
            }
            else
            {
                Debug.Log("onbuttclick");
                AudioSource Round_list_count = sound_game.Round_list[count_question];
                Round_list_count.Stop(); // останавливаем звук, который будет играть в течении раунда
                // появляется плашка на сцене с суммой
                // !!!!
                //sound_game.count_q++;
                //count_question++;
                // !!!!
                StartCoroutine(Amount_Bar(count_question));
                // StopCoroutine(Amount_Bar(count_question)); (как вариант, я не пробовал)

                //SetQuestion();
            }
            //Debug.Log("Correct");
        }
        else
        {
            switch (correctIndex)
            {
                case 0:
                    Answ_a.color = Color.white;
                    break;
                case 1:
                    Answ_b.color = Color.white;
                    break;
                case 2:
                    Answ_c.color = Color.white;
                    break;
                case 3:
                    Answ_d.color = Color.white;
                    break;
            }
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // функция перезагружает сцену
            AudioSource Round_list_co = sound_game.Round_list[count_question];
            Round_list_co.Stop(); // останавливаем звук, который будет играть в течении раунда
            StartCoroutine(Player_lose(count_question));
            //EndGame();
        }
        return correctIndex; // для чего нужен?
    }

    public byte GetCurrentCorrectIndex()
    {
        return _questions[_currentIndex].CorrectIndex;
    }


    private void Test()
    {
        for (byte i = 0; i < _answerButtons.Length; i++)
        {
            byte index = i;
            _answerButtons[i].onClick.AddListener(() => OnButtonClick(index)); // onbuttonclick(скобки)
        }
    }
    private void Start()
    {
        Begin_Game();
        SetQuestion();
        Test();
    }

    private void Test_func(int count)
    {
        // вызываю функцию start из друго скрипта
        sound_game.Myfunc(); // зачем здесь эта функция?
        sound_game.func(count);
    }

    IEnumerator Amount_Bar(int count)
    {
        //Debug.Log("Сейчас в count::" + count);
        //Debug.Log("Сработала функция Round_list_0.Stop() gamecontroller ");
        yield return new WaitForSeconds(3.5f);
        AudioSource final_otvet = sound_game.sound_final_answer[count];
        Debug.Log("Сработала функция Round_list_0.Stop() gamecontroller " + count);
        final_otvet.Stop();


        _questionText.enabled = false;
        label_question.enabled = false;
        Answ_a.enabled = false;
        Answ_b.enabled = false;
        Answ_c.enabled = false;
        Answ_d.enabled = false;
        

        for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].enabled = false;
        }
        // появляется вектор -> плашка суммы (при помощи вектора)
        sum_bar.rectTransform.anchoredPosition = new Vector2(6.0916e-05f, -308.01f);
        Image x2 = lifeline_x2.x2_pos;
        x2.rectTransform.anchoredPosition = new Vector2(833.79f, -682);
        amount_bar.text = total_prize[count];
        yield return new WaitForSeconds(1f);
        sum_bar.rectTransform.anchoredPosition = new Vector2(978.55f, -1483);
        Test_func(count);
        SetQuestion();
        flashok = true;
        yield break;
    }

    IEnumerator Player_lose(int count)
    {
        Debug.Log("Сработала фукция Player_lose");
        yield return new WaitForSeconds(4.5f);
        // играет звук покидания игрока
        SceneManager.LoadScene("display_lose");
        yield break;
    }

}
