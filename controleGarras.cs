//  Developed by AW Simuladores Ltda

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class controleGarras : MonoBehaviour
{
    [Header("Input")]
    public PlayerInput AWInput;

    [Header("Garras e Rolos")]
    public Rigidbody GarraSuperiorEsquerda;
    public Rigidbody GarraSuperiorDireita;
    public Rigidbody GarraInferiorEsquerda;
    public Rigidbody GarraInferiorDireita;
    public Rigidbody BracoBaseRoloEsquerdo;
    public Rigidbody BracoBaseRoloDireito;
    public GameObject SegundoBracoRotatorEsquerdo;
    public GameObject SegundoBracoRotatorDireito;
    public GameObject RoloEsquerdo;
    public GameObject RoloDireito;

    private HingeJoint hinge1;
    private HingeJoint hinge2;
    private HingeJoint hinge3;
    private HingeJoint hinge4;
    private HingeJoint hinge5;
    private HingeJoint hinge6;
    private HingeJoint hingeBasePrincipal;

    private float GarraSuperiorEsquerdaFloat;
    private float GarraSuperiorDireitaFloat;
    private float GarraInferiorEsquerdaFloat;
    private float GarraInferiorDireitaFloat;
    private float BracoBaseRoloEsquerdoFloat;
    private float BracoBaseRoloDireitoFloat;

    [Header("CABECOTE")]
    public GameObject Cabecote;
    private float cabecoteIvertido;

    [Header("GUI")]
    public TextMeshProUGUI metragemDefinidaTxt;
    public TextMesh metragemDefinidaTxtReal;
    public TextMeshProUGUI metragemTxt;
    public TextMesh metragemTxtReal;

    [Header("Outros")]
    public Toggle InverterCabecote;
    private GameObject ArvoreNoCabecote;
    private float sensibilidade;
    private HingeJoint hingeCab;
    private AudioSource SomDelimb;
        private bool SomTiltUpBool = false;
    public AudioSource SomTiltUp;
    public GameObject BasePrincipal;
    public GameObject RotatorCab;

    [SerializeField]
    public GameObject arvoreParent;
    public GameObject AudioAbrirGarras;
    public GameObject AudioAbrirRolos;
    public GameObject AudioFecharGarras;
    public GameObject AudioFecharRolos;

    public GameObject corrente;
    public GameObject cortadorCorrente;
    public GameObject cortadorEnfeite;
    public Transform parent;
    public Transform parent2;
    public bool TemToraDentro;
    public bool tora1 = false;
    public bool tora2 = false;
    internal static float MetragemDefinida = 6f;
    internal float atualMetragem = 0f;
    internal bool cortando = false;
    internal float Metragem = 0;
    internal float metragemDisplay = 0;
    internal float movimentacao = 0.1f;
    internal float movimentacaoMetragem = 0.1f;
    internal float movRotatorDir = -0.1f;
    internal float movRotatorEsq = 0.1f;

    [SerializeField]
    internal float VelocidadeMovimento = 0.2f;
    internal float velocidadeRotacao = 2.0f;
    internal float velocidadeRotacao2 = -2.0f;
    public PhysicMaterial Grip;
    public PhysicMaterial Arvore;
    public PhysicMaterial ToraMaterial;
    private bool AtmAtivado = true;
    private HingeJoint hinge;
    private JointSpring hingeSpring;

    private float CabecoteFloat = 0;

    public void RemoverMola()
    {
        // hinge.useSpring = false;
    }

    public void TiltDownAutomatico()
    {
        var motor = hinge.motor;
        motor.targetVelocity = 40;
        hinge.motor = motor;
    }

    internal void Cortando()
    {
        cortando = false;
    }

    internal void Cortar()
    {
        FecharFacas();
        FecharRolos();
        Metragem = atualMetragem - MetragemDefinida;
        if (metragemTxt) 
        metragemTxt.text = metragemDisplay.ToString("0000");
        if (metragemTxtReal)
        metragemTxtReal.text = metragemDisplay.ToString("0000");
        movimentacaoMetragem = 0;
        metragemDisplay = 0;
        Metragem = atualMetragem - MetragemDefinida;
        triggerTora.diametroMadeira = 0;

        tora2 = false;
        cortadorEnfeite.SetActive(false);
        cortadorCorrente.SetActive(true);

        StartCoroutine(CortarVerdade());

        StartCoroutine(ResetarCortador());
    }

    internal IEnumerator CortarVerdade()
    {
        yield return new WaitForSeconds(1f);

        corrente.SetActive(true);
        StartCoroutine(DesativarCorrenteNaVolta());
    }

    internal IEnumerator DesativarCorrenteNaVolta()
    {
        yield return new WaitForSeconds(0.5f);

        corrente.SetActive(false);
    }

    internal IEnumerator ResetarCortador()
    {
        yield return new WaitForSeconds(2f);

        cortadorCorrente.SetActive(false);
        cortadorEnfeite.SetActive(true);
    }

    internal void Start()
    {
        //cortador
        SomDelimb = GetComponent<AudioSource>();
        cortadorEnfeite.SetActive(true);
        cortadorCorrente.SetActive(false);
        corrente.SetActive(false);

        hinge1 = GarraSuperiorEsquerda.GetComponent<HingeJoint>();
        hinge2 = GarraSuperiorDireita.GetComponent<HingeJoint>();
        hinge3 = GarraInferiorEsquerda.GetComponent<HingeJoint>();
        hinge4 = GarraInferiorDireita.GetComponent<HingeJoint>();
        hinge5 = BracoBaseRoloEsquerdo.GetComponent<HingeJoint>();
        hinge6 = BracoBaseRoloDireito.GetComponent<HingeJoint>();
        hingeBasePrincipal = BasePrincipal.GetComponent<HingeJoint>();

        hinge = Cabecote.GetComponent<HingeJoint>();
        hingeCab = RotatorCab.GetComponent<HingeJoint>();

        SegundoBracoRotatorEsquerdo.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloEsquerdo.transform.localRotation.y * -120,
            0
        );
        SegundoBracoRotatorDireito.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloDireito.transform.localRotation.y * 120,
            0
        );

        GarraSuperiorEsquerdaFloat = GarraSuperiorEsquerda.transform.localEulerAngles.y;
        GarraSuperiorDireitaFloat = GarraSuperiorDireita.transform.localEulerAngles.y;
        GarraInferiorEsquerdaFloat = GarraInferiorEsquerda.transform.localEulerAngles.y;
        GarraInferiorDireitaFloat = GarraInferiorDireita.transform.localEulerAngles.y;
        BracoBaseRoloEsquerdoFloat = BracoBaseRoloEsquerdo.transform.localEulerAngles.y;
        BracoBaseRoloDireitoFloat = BracoBaseRoloDireito.transform.localEulerAngles.y;
    }

    public void Invertidos()
    {
        if (InverterCabecote.isOn)
        {
            cabecoteIvertido = -1;
        }
        else
        {
            cabecoteIvertido = 1;
        }
    }

    internal void TiltDown()
    {
        var motor = hinge.motor;
        motor.targetVelocity = 40;
        hinge.motor = motor;

        tora2 = false;
    }

    void FecharFacas()
    {
        hinge1.useMotor = true;
        hinge2.useMotor = true;
        hinge3.useMotor = true;
        hinge4.useMotor = true;
        var motor1 = hinge1.motor;
	    motor1.targetVelocity = 15000 * Time.deltaTime;
        hinge1.motor = motor1;
        var motor2 = hinge2.motor;
        motor2.targetVelocity = 15000 * -Time.deltaTime;
        hinge2.motor = motor2;
        var motor3 = hinge3.motor;
        motor3.targetVelocity = 15000 * Time.deltaTime;
        hinge3.motor = motor3;
        var motor4 = hinge4.motor;
        motor4.targetVelocity = 15000 * -Time.deltaTime;
        hinge4.motor = motor4;

        GarraSuperiorEsquerdaFloat = GarraSuperiorEsquerda.transform.localEulerAngles.y;
        GarraSuperiorDireitaFloat = GarraSuperiorDireita.transform.localEulerAngles.y;
        GarraInferiorEsquerdaFloat = GarraInferiorEsquerda.transform.localEulerAngles.y;
        GarraInferiorDireitaFloat = GarraInferiorDireita.transform.localEulerAngles.y;
    }

    void FecharRolos()
    {
        hinge5.useMotor = true;

        hinge6.useMotor = true;
        var motor5 = hinge5.motor;
        motor5.targetVelocity = 15000 * Time.deltaTime;
        hinge5.motor = motor5;
        var motor6 = hinge6.motor;
        motor6.targetVelocity = 15000 * -Time.deltaTime;
        hinge6.motor = motor6;

        SegundoBracoRotatorEsquerdo.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloEsquerdo.transform.localRotation.y * -120,
            0
        );
        SegundoBracoRotatorDireito.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloDireito.transform.localRotation.y * 120,
            0
        );
        BracoBaseRoloEsquerdoFloat = BracoBaseRoloEsquerdo.transform.localEulerAngles.y - 3;
        BracoBaseRoloDireitoFloat = BracoBaseRoloDireito.transform.localEulerAngles.y + 3;
    }

    void AbrirFacas()
    {
        hinge1.useMotor = true;
        hinge2.useMotor = true;
        hinge3.useMotor = true;
        hinge4.useMotor = true;

        var motor1 = hinge1.motor;
        motor1.targetVelocity = 20000 * -Time.deltaTime;
        hinge1.motor = motor1;
        var motor2 = hinge2.motor;
        motor2.targetVelocity = 20000 * Time.deltaTime;
        hinge2.motor = motor2;

        var motor3 = hinge3.motor;
        motor3.targetVelocity = 20000 * -Time.deltaTime;
        hinge3.motor = motor3;
        var motor4 = hinge4.motor;
        motor4.targetVelocity = 20000 * Time.deltaTime;
        hinge4.motor = motor4;

        GarraSuperiorEsquerdaFloat = GarraSuperiorEsquerda.transform.localEulerAngles.y;
        GarraSuperiorDireitaFloat = GarraSuperiorDireita.transform.localEulerAngles.y;
        GarraInferiorEsquerdaFloat = GarraInferiorEsquerda.transform.localEulerAngles.y;
        GarraInferiorDireitaFloat = GarraInferiorDireita.transform.localEulerAngles.y;
    }

    void AbrirRolos()
    {
        hinge5.useMotor = true;
        var motor5 = hinge5.motor;
        motor5.targetVelocity = 20000 * -Time.deltaTime;
        hinge5.motor = motor5;

        hinge6.useMotor = true;
        var motor6 = hinge6.motor;
        motor6.targetVelocity = 20000 * Time.deltaTime;
        hinge6.motor = motor6;

        BracoBaseRoloEsquerdoFloat = BracoBaseRoloEsquerdo.transform.localEulerAngles.y;
        BracoBaseRoloDireitoFloat = BracoBaseRoloDireito.transform.localEulerAngles.y;

        SegundoBracoRotatorEsquerdo.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloEsquerdo.transform.localRotation.y * -120,
            0
        );
        SegundoBracoRotatorDireito.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloDireito.transform.localRotation.y * 120,
            0
        );
    }

    void FacasERolosParados()
    {
        var motor1 = hinge1.motor;
        motor1.targetVelocity = 0;
        hinge1.motor = motor1;
        hinge1.useMotor = false;

        var motor2 = hinge2.motor;
        motor2.targetVelocity = 0;
        hinge2.motor = motor2;
        hinge2.useMotor = false;

        var motor3 = hinge3.motor;
        motor3.targetVelocity = 0;
        hinge3.motor = motor3;
        hinge3.useMotor = false;

        var motor4 = hinge4.motor;
        motor4.targetVelocity = 0;
        hinge4.motor = motor4;
        hinge4.useMotor = false;

        var motor5 = hinge5.motor;
        motor5.targetVelocity = 0;
        hinge5.motor = motor5;
        hinge5.useMotor = false;

        var motor6 = hinge6.motor;
        motor6.targetVelocity = 0;
        hinge6.motor = motor6;
        hinge6.useMotor = false;

        GarraSuperiorEsquerda.transform.localEulerAngles = new Vector3(
            0,
            GarraSuperiorEsquerdaFloat,
            0
        );
        GarraSuperiorDireita.transform.localEulerAngles = new Vector3(
            0,
            GarraSuperiorDireitaFloat,
            0
        );
        GarraInferiorEsquerda.transform.localEulerAngles = new Vector3(
            0,
            GarraInferiorEsquerdaFloat,
            0
        );
        GarraInferiorDireita.transform.localEulerAngles = new Vector3(
            0,
            GarraInferiorDireitaFloat,
            0
        );
        BracoBaseRoloEsquerdo.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloEsquerdoFloat,
            0
        );
        BracoBaseRoloDireito.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloDireitoFloat,
            0
        );

        SegundoBracoRotatorEsquerdo.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloEsquerdo.transform.localRotation.y * -120,
            0
        );
        SegundoBracoRotatorDireito.transform.localEulerAngles = new Vector3(
            0,
            BracoBaseRoloDireito.transform.localRotation.y * 120,
            0
        );
    }

    void ControleGarras()
    {
        if (AWInput.actions["AbrirFacasFecharRolos"].ReadValue<float>() != 0)
        {
            AbrirFacas();
            FecharRolos();
        }
        if (AWInput.actions["AbrirRolosFecharFacas"].ReadValue<float>() != 0)
        {
            FecharFacas();
            AbrirRolos();
        }
        if (AWInput.actions["FecharFacasERolos"].ReadValue<float>() != 0)
        {
            FecharFacas();
            FecharRolos();
        }

        if (AWInput.actions["AbrirFacasERolos"].ReadValue<float>() != 0)
        {
            AbrirFacas();
            AbrirRolos();
        }

        if (
            AWInput.actions["AbrirFacasERolos"].ReadValue<float>() == 0
            && AWInput.actions["FecharFacasERolos"].ReadValue<float>() == 0
            && AWInput.actions["RoloFrente"].ReadValue<float>() == 0
            && AWInput.actions["RoloTras"].ReadValue<float>() == 0
            && AWInput.actions["AbrirFacasFecharRolos"].ReadValue<float>() == 0
            && AWInput.actions["AbrirRolosFecharFacas"].ReadValue<float>() == 0
            && AWInput.actions["Automatico"].ReadValue<float>() == 0
            && AWInput.actions["TiltUp"].ReadValue<float>() == 0
            && AWInput.actions["Serra"].ReadValue<float>() == 0
        )
        {
            FacasERolosParados();
        }
    }

    void SomGarrasERolos()
    {
        if (SomTiltUpBool == true)
        {
            if (hingeBasePrincipal.transform.localEulerAngles.x > 353f)
            {
                SomTiltUp.Play();              

            }
        }
        

        if (GarraSuperiorEsquerda.transform.localEulerAngles.y < 18)
        {
            AudioAbrirGarras.SetActive(true);
        }
        else
        {
            AudioAbrirGarras.SetActive(false);
        }

        if (GarraSuperiorEsquerda.transform.localEulerAngles.y > 92.9f)
        {
            AudioFecharGarras.SetActive(true);
        }
        else
        {
            AudioFecharGarras.SetActive(false);
        }

        if (BracoBaseRoloEsquerdo.transform.localEulerAngles.y < 12.1f)
        {
            AudioAbrirRolos.SetActive(true);
        }
        else
        {
            AudioAbrirRolos.SetActive(false);
        }

        if (BracoBaseRoloEsquerdo.transform.localEulerAngles.y > 94.9f)
        {
            AudioFecharRolos.SetActive(true);
        }
        else
        {
            AudioFecharRolos.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        sensibilidade = AWInputUIButtons.sensibilidadeGeral;
        Invertidos();
        Descascar();
        ControleGarras();
        SomGarrasERolos();

        float medtragemFinalDefinida = MetragemDefinida * 1000;
        if(metragemDefinidaTxt)
        metragemDefinidaTxt.text = medtragemFinalDefinida.ToString();
        if(metragemDefinidaTxtReal)
        metragemDefinidaTxtReal.text = medtragemFinalDefinida.ToString();

        ///////////////////////////////////////////////////////////


        if (AWInput.actions["Cabecote"].ReadValue<float>() != 0)
        {
            hingeCab.useMotor = true;
            var motorCab = hingeCab.motor;

            motorCab.targetVelocity =
                70
                * cabecoteIvertido
                * sensibilidade
                * AWInput.actions["Cabecote"].ReadValue<float>();
            hingeCab.motor = motorCab;
            CabecoteFloat = RotatorCab.transform.localEulerAngles.y;
        }
        else
        {
            hingeCab.useMotor = false;
            var motorCab = hingeCab.motor;
            motorCab.targetVelocity = 0;
            hingeCab.motor = motorCab;
            //  RotatorCab.transform.localEulerAngles = new Vector3(0, CabecoteFloat, 0);
        }

        if (AWInput.actions["AtivarAutomatico"].ReadValue<float>() != 0)
        {
            AtmAtivado = true;
        }

        ///////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////




        TemToraDentro = triggerTora.TemTora;
        tora1 = TemToraDentro;

        ///////////////////////////////////////////////////////////
        if (AWInput.actions["ResetarMadeira"].ReadValue<float>() != 0)
        {
            movimentacaoMetragem = 0;
            metragemDisplay = 0;
            Metragem = atualMetragem - MetragemDefinida;
            triggerTora.diametroMadeira = 0;
        }

        ///////////////////////////////////////////////////////////
        if (AWInput.actions["RoloTras"].ReadValue<float>() != 0)
        {
            movimentacaoMetragem -=
                VelocidadeMovimento * AWInput.actions["RoloTras"].ReadValue<float>() / 3f;

            metragemDisplay = movimentacaoMetragem * 1000;
            movRotatorDir += VelocidadeMovimento * AWInput.actions["RoloTras"].ReadValue<float>();
            movRotatorEsq -= VelocidadeMovimento * AWInput.actions["RoloTras"].ReadValue<float>();

            RoloEsquerdo.transform.localEulerAngles = new Vector3(0, 0, movRotatorEsq * 50);
            RoloDireito.transform.localEulerAngles = new Vector3(0, 0, movRotatorDir * 50);
            SomDelimb.mute = false;

            FecharFacas();
            FecharRolos();
            if (ArvoreNoCabecote)
            {
                ArvoreNoCabecote.transform.Translate(
                    Vector3.up * AWInput.actions["RoloTras"].ReadValue<float>() / 15
                );
            }
        }
        else if (AWInput.actions["RoloFrente"].ReadValue<float>() != 0)
        {
            FecharFacas();
            FecharRolos();
            if (ArvoreNoCabecote)
            {
                ArvoreNoCabecote.transform.Translate(
                    Vector3.down * AWInput.actions["RoloFrente"].ReadValue<float>() / 15
                );
            }

            movimentacaoMetragem +=
                VelocidadeMovimento * AWInput.actions["RoloFrente"].ReadValue<float>() / 3f;

            metragemDisplay = movimentacaoMetragem * 1000;
            movRotatorDir += VelocidadeMovimento * AWInput.actions["RoloFrente"].ReadValue<float>();
            movRotatorEsq -= VelocidadeMovimento * AWInput.actions["RoloFrente"].ReadValue<float>();

            RoloEsquerdo.transform.localEulerAngles = new Vector3(0, 0, movRotatorEsq * 50);
            RoloDireito.transform.localEulerAngles = new Vector3(0, 0, movRotatorDir * 50);
            SomDelimb.mute = false;
        }
        else
        {
            SomDelimb.mute = true;
        }

        ///////////////////////////////////////////////////////////
        if (AWInput.actions["TiltUp"].ReadValue<float>() != 0)
        {
            SomTiltUpBool = true;
            AbrirRolos();
            AbrirFacas();
            StartCoroutine(TiltUp());
        }

        ///////////////////////////////////////////////////////////
        if (AWInput.actions["TiltDown"].ReadValue<float>() != 0)
        {
            SomTiltUpBool = false;
            TiltDown();
        }
        ///////////////////////////////////////////////////////////
        ///

        if (AWInput.actions["Serra"].ReadValue<float>() > 0.1f)
        {
            FecharRolos();
            FecharFacas();
            if (tora2)
            {
                hingeSpring.spring = 70000;
                hingeSpring.damper = 7000;
                hingeSpring.targetPosition = -10;
                hinge.spring = hingeSpring;
                hinge.useSpring = true;
            }
            AtmAtivado = true;
            Cortar();            
            Invoke("TiltDownAutomatico", 1f);
            
        }

        ///////////////////////////////////////////////////////////
        // Automatico
        if (AWInput.actions["Automatico"].ReadValue<float>() != 0 && AtmAtivado == true)
        {
            FecharFacas();
            FecharRolos();
            if (tora2 == true)
            {
                cortando = true;
                Cortar();

                Invoke("Cortando", 2f);
                Invoke("TiltDownAutomatico", 1f);
            }
            else if (atualMetragem <= Metragem)
            {
                cortando = true;
                Invoke("TiltDownAutomatico", 1f);

                Cortar();

                Invoke("Cortando", 2f);
                Invoke("RemoverMola", 2);
            }
            if (cortando == false && tora2 == false)
            {
                movimentacao -=
                    VelocidadeMovimento * AWInput.actions["Automatico"].ReadValue<float>() / 3f;
                movimentacaoMetragem +=
                    VelocidadeMovimento * AWInput.actions["Automatico"].ReadValue<float>() / 3f;

                movRotatorDir +=
                    VelocidadeMovimento * AWInput.actions["Automatico"].ReadValue<float>();
                movRotatorEsq -=
                    VelocidadeMovimento * AWInput.actions["Automatico"].ReadValue<float>();

                RoloEsquerdo.transform.localEulerAngles = new Vector3(0, 0, movRotatorEsq * 50);
                RoloDireito.transform.localEulerAngles = new Vector3(0, 0, movRotatorDir * 50);
                if (ArvoreNoCabecote)
                {
                    ArvoreNoCabecote.transform.Translate(
                        Vector3.down * AWInput.actions["Automatico"].ReadValue<float>() / 15
                    );
                }
                atualMetragem = movimentacao;
                metragemDisplay = movimentacaoMetragem * 1000;
                SomDelimb.mute = false;
            }
            else
            {
                SomDelimb.mute = true;
            }
        }

        if (metragemDisplay == 0)
        {   if(metragemTxt)
            metragemTxt.text = metragemDisplay.ToString("0");
            if (metragemTxtReal) 
            metragemTxtReal.text = metragemDisplay.ToString("0");
        }
        else
        {
            if (metragemTxt)
            metragemTxt.text = metragemDisplay.ToString("0000");
            if (metragemTxtReal)
            metragemTxtReal.text = metragemDisplay.ToString("0000");
        }
    }

    IEnumerator TiltUp()
    {
        yield return new WaitForSeconds(0.5f);

        AtmAtivado = true;
        movimentacaoMetragem = 0;
        metragemDisplay = 0;
        Metragem = atualMetragem - MetragemDefinida;
        triggerTora.diametroMadeira = 0;
        /// +1 madeira (No tilti up ele contabiliza as madeiras)
        var motor = hinge.motor;
        motor.targetVelocity = -50;
        hinge.motor = motor;
    }

    public void OnTriggerEnter(Collider other)
    {
        //if the object is not already in the list

        if (other.gameObject.tag == "Arvore")
        {
            tora2 = true;
        }

        if (other.gameObject.tag == "Tora")
        {
            tora1 = true;
            other.gameObject.GetComponent<MeshCollider>().material = ToraMaterial;
            ArvoreNoCabecote = other.gameObject;
        }else if (other.gameObject.tag == "Tora2")
        {
	        tora1 = true;
	        other.gameObject.GetComponent<MeshCollider>().material = ToraMaterial;
	        ArvoreNoCabecote = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Arvore")
        {
            tora2 = false;
        }

        if (other.gameObject.tag == "Tora")
        {
            ArvoreNoCabecote = null;
            other.gameObject.GetComponent<MeshCollider>().material = null;
            descascou = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Arvore")
        {
            tora2 = true;
        }

        if (other.gameObject.tag == "Tora")
        {
            tora1 = true;
            ArvoreNoCabecote = other.gameObject;
        }
    }

    bool descascou = false;

    void Descascar()
    {
        if (tora1 == true && metragemDisplay > 10000)
        {
            descascou = true;
        }
        if (descascou == true && metragemDisplay <= 1000)
        {
            Material yourMaterial = Resources.Load<Material>("arvoreGalhoDesc");
            Debug.LogWarning("DESCAS");
            if (ArvoreNoCabecote)
            {
                ArvoreNoCabecote.GetComponent<Renderer>().material.color = Color.white;
            }
            descascou = false;
        }
    }
}
