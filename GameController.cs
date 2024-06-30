using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    [SerializeField] private Button[] _answerButtons;
    private TMP_Text[] _answerButtonsText;

    [Header("Tips")]
    [SerializeField] private Button _tip50; // 50:50
    [SerializeField] private Button _tipCall; // звонок другу
    [SerializeField] private Button _tiePeople; // Помощь зала
    [SerializeField] private Button _tiex2; // право на ошибку


    [Header("Questions")]
    [SerializeField] private Question[] _questions;


    [Header("Components")]
    public SpriteRenderer label_question; // синяя плашка, включающая в себя вопрос и ответ
    public TMP_Text Answ_a;
    public TMP_Text Answ_b;
    public TMP_Text Answ_c;
    public TMP_Text Answ_d;
    public Image sum_bar;
    public Text amount_bar;
    public Sound_game sound_game;

    /*
     * Переменные
     */
    private string[] total_prize = { // массив используется для плашки суммы
        "0",
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
    }
    private void EndGame()
    {
        Debug.Log("Игра окончена");
    }


    public byte OnButtonClick(byte index)
    {
        int count_question = sound_game.count_q;
        var correctIndex = _questions[_currentIndex].CorrectIndex;
        AudioSource final_otvet = sound_game.sound_final_answer[count_question];
        final_otvet.Play();
        if (index == correctIndex)
        {
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
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // функция перезагружает сцену
            AudioSource Round_list_co = sound_game.Round_list[count_question];
            Round_list_co.Stop(); // останавливаем звук, который будет играть в течении раунда
            StartCoroutine(Amount_Bar(count_question));
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
            var index = i;
            _answerButtons[i].onClick.AddListener(() => OnButtonClick(index));
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
        sound_game.Myfunc();
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
        // появляется вектор -> плашка суммы (при помощи вектора)
        sum_bar.rectTransform.anchoredPosition = new Vector2(6.0916e-05f, -308.01f);
        amount_bar.text = total_prize[count];
        yield return new WaitForSeconds(1f);
        sum_bar.rectTransform.anchoredPosition = new Vector2(978.55f, -1483);
        Test_func(count);
        SetQuestion();
    }

}