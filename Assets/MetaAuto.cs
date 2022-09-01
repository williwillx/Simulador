using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;

public class MetaAuto : MonoBehaviour
{
    public Text txDescricao;
    public Text txTirulo;
    public Image progresbar;
    public AudioSource audioSource;


    public GameObject Carro;
    public GameObject Suspensao;
    public GameObject Motor;
    public GameObject Tanque;
    public GameObject Smoke;
    public GameObject Tank;
    public GameObject rod1;
    public GameObject rod2;
    public GameObject rod3;
    public GameObject rod4;
    public GameObject pistao1;
    public GameObject pistao2;
    public GameObject pistao3;
    public GameObject pistao4;

    int step = 0;

    //Azure SpeechService
    private const string SubscriptionKey = "e382b945493d413288c194ce1a2e4fda";
    private const string Region = "brazilsouth";
    private const int SampleRate = 24000;
    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;


    public Transform cameraPosition1;
    public Transform cameraLookAtOn1;

    public Transform cameraPosition2;
    public Transform cameraLookAtOn2;

    public Transform cameraPosition3;
    public Transform cameraLookAtOn3;

    public Transform cameraPosition4;
    public Transform cameraLookAtOn4;

    public void ButtonClick()
    {
    
        string newMessage = null;
        var startTime = DateTime.Now;

        
        using (var result = synthesizer.StartSpeakingTextAsync(txDescricao.text).Result)
        {
            
            var audioDataStream = AudioDataStream.FromResult(result);
            var isFirstAudioChunk = true;
            var audioClip = AudioClip.Create(
                "Speech",
                SampleRate * 600, // Can speak 10mins audio as maximum
                1,
                SampleRate,
                true,
                (float[] audioChunk) =>
                {
                    var chunkSize = audioChunk.Length;
                    var audioChunkBytes = new byte[chunkSize * 2];
                    var readBytes = audioDataStream.ReadData(audioChunkBytes);
                    if (isFirstAudioChunk && readBytes > 0)
                    {
                        var endTime = DateTime.Now;
                        var latency = endTime.Subtract(startTime).TotalMilliseconds;
                        newMessage = $"Speech synthesis succeeded!\nLatency: {latency} ms.";
                        isFirstAudioChunk = false;
                    }

                    for (int i = 0; i < chunkSize; ++i)
                    {
                        if (i < readBytes / 2)
                        {
                            audioChunk[i] = (short)(audioChunkBytes[i * 2 + 1] << 8 | audioChunkBytes[i * 2]) / 32768.0F;
                        }
                        else
                        {
                            audioChunk[i] = 0.0f;
                        }
                    }

               
                });

            audioSource.clip = audioClip;
            audioSource.Play();
        }


    }

    void Start()
    {
            speechConfig = SpeechConfig.FromSubscription(SubscriptionKey, Region);
            speechConfig.SpeechRecognitionLanguage = "pt-BR";
            speechConfig.SpeechSynthesisVoiceName = "pt-BR-FranciscaNeural"; 
            speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw24Khz16BitMonoPcm);
            synthesizer = new SpeechSynthesizer(speechConfig, null);
            synthesizer.SynthesisCanceled += (s, e) =>
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(e.Result);             
            };    
    }


    public void ChangeText(int count)
    {
        TransparencyParent _carro = Carro.GetComponent<TransparencyParent>();
        TransparencyParent _suspensao = Suspensao.GetComponent<TransparencyParent>();
        TransparencyParent _motor = Motor.GetComponent<TransparencyParent>();
        TransparencyParent _tanque = Tanque.GetComponent<TransparencyParent>();
        ChangeColorParticle _smoke = Smoke.GetComponent<ChangeColorParticle>();
        TransparencyChild _rod1 = rod1.GetComponent<TransparencyChild>();
        TransparencyChild _rod2 = rod2.GetComponent<TransparencyChild>();
        TransparencyChild _rod3 = rod3.GetComponent<TransparencyChild>();
        TransparencyChild _rod4 = rod4.GetComponent<TransparencyChild>();
        TransparencyChild _pistao1 = pistao1.GetComponent<TransparencyChild>();
        TransparencyChild _pistao2 = pistao2.GetComponent<TransparencyChild>();
        TransparencyChild _pistao3 = pistao3.GetComponent<TransparencyChild>();
        TransparencyChild _pistao4 = pistao4.GetComponent<TransparencyChild>();
        step += count;
        if(step == 5)
        {
            step = 4;
        }
        else if (step == 0)
        {
            step = 1;
        }
        txTirulo.text = "Passo " + step;
        switch (step)
        {
            case 1:
                Smoke.SetActive(false);
                _carro.DisableTransparency();
                _suspensao.DisableTransparency(); 
                txDescricao.text = "BEM VINDO AO SIMULADOR AUTOMOTIVO";
                progresbar.fillAmount = 0.25f;
                SetupCam(cameraPosition1, cameraLookAtOn1);
                break;
            case 2:
                txDescricao.text = "O TANQUE DE COMBUSTÍVEL É RESPONSAVEL POR ARMAZENAR GASOLINA PARA QUEIMAR NO MOTOR DO VEÍCULO";
                SetupCam(cameraPosition2, cameraLookAtOn2);
                _carro.EnableTransparency();
                _tanque.DisableTransparency();
                _suspensao.EnableTransparency();
                _motor.EnableTransparency();
                progresbar.fillAmount = 0.50f;          
                break;
            case 3:
                Smoke.SetActive(false);
                txDescricao.text = "A QUEIMA DE COMBUSTÍVEL OCORRE NO INTERIOR DA CÂMARA DE COMBUSTÃO!";
                SetupCam(cameraPosition3, cameraLookAtOn3);
                progresbar.fillAmount = 0.75f;
                _carro.EnableTransparency();
                _tanque.EnableTransparency();
                _suspensao.EnableTransparency();
                _rod1.DisableTransparency();
                _rod2.DisableTransparency();
                _rod3.DisableTransparency();
                _rod4.DisableTransparency();
                _pistao1.DisableTransparency();
                _pistao2.DisableTransparency();
                _pistao3.DisableTransparency();
                _pistao4.DisableTransparency();
                break;
            case 4:
                txDescricao.text = "LIBERANDO O RESULTADO DA QUEIMA PELO ESCAPAMENTO. OBRIGADO POR ASSISTIR ESSA SIMULAÇÃO";
                Smoke.SetActive(true);
                progresbar.fillAmount = 1f;
                SetupCam(cameraPosition4, cameraLookAtOn4);
                _smoke.ChangeColor();
                _carro.DisableTransparency();
                _suspensao.DisableTransparency();
                break;
        }
        ButtonClick();
    }

    void SetupCam(Transform _pos, Transform _lookAt)
    {
        GameObject cameraGO = Camera.main.gameObject;
        CameraController _cameraController = cameraGO.GetComponent<CameraController>();
        
        // freeCamera.enabled = false;
        _cameraController.MoveCameraTo(_pos, _lookAt, 0.25f, 4);
        
    }

    public void stop()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }

    void OnDestroy()
    {
        if (synthesizer != null)
        {
            synthesizer.Dispose();
        }
    }
}

