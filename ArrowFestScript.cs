using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using System;

public class ArrowFestScript : MonoBehaviour
{
    public List<GameObject> arrows = new List<GameObject>();
    public GameObject arrowObject;
    public Transform Parent;
    public float MinX, MaksX;
    public LayerMask layermask;
    public float mesafe;


    [Range(0,300)]
    public int ArrowCount;

    bool isDecrase;



    // *!!!*

    // Arrow Fest Oyun Mekanigi
    // Hata Veya Soru sormak istiyorsaniz Discord DRAÇ#7981 mesaj gonderebilirsiniz

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GetRay();
        }
    }

    void GetRay()
    {
        Vector3 MousePos = Input.mousePosition;
        MousePos.z = Camera.main.transform.position.z;

        Ray ray = Camera.main.ScreenPointToRay(MousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layermask))
        {
            Vector3 mouse = hit.point;
            mouse.x = Mathf.Clamp(mouse.x, MinX, MaksX); //Minx v MaksX arasýnda deðer al

            mesafe = mouse.x;

            diz();
        }
    }

    void diz()
    {
        float angle = 1f;

        float arrowCount = arrows.Count;

        angle = 360 / arrowCount;

        for (int i = 0; i < arrowCount; i++)
        {
            MoveObjects(arrows[i].transform, i * angle);

        }

    }
    void MoveObjects(Transform objectTransform,float degree)
    {
        Vector3 pos = Vector3.zero;

        pos.x = Mathf.Cos(degree * Mathf.Deg2Rad);
        pos.y = Mathf.Sin(degree * Mathf.Deg2Rad);

        objectTransform.localPosition = pos * mesafe;
    }



     
    // *!!!*

    // Editor Ayarlari


    // Oyun calismiyorken editor penceresinde bir degisiklik yapildiginda ornegin (Arrow sayisini arttirdigimizda) asagidaki OnValidate fonksiyonu calisir
    // Yani oyunu calistirmadan Editordern ayar yapmaya yarar
    // Ama bunu kullanmak istiyorsaniz Package Manager'den Editor Coroutines paketini idnirmelisiniz

    private void OnValidate()
    {
        if (ArrowCount > arrows.Count && !isDecrase)
        {
            CreateArrow();
        }
        else if(arrows.Count > ArrowCount)
        {
            isDecrase = true;
            DestroyArrows();
        }
        else
        {
            diz();
        }
    }


    private void CreateArrow()
    {
        for (int i = arrows.Count; i < ArrowCount; i++)
        {
            GameObject arrowClone = Instantiate(arrowObject, Parent);
            arrows.Add(arrowClone);
            arrowClone.transform.localPosition = Vector3.zero;
        }

        diz();
    }

    private void DestroyArrows()
    {
        for (int i = arrows.Count - 1; i >= ArrowCount; i--)
        {
            GameObject arrowClone = arrows[i];
            arrows.RemoveAt(i);
            EditorCoroutineUtility.StartCoroutine(DestroyObject(arrowClone), this); //Destroy islemini editorde yapmak istiyorsaniz bunu kullanmalisiniz
        }

        isDecrase = false;
        diz();

    }

    IEnumerator DestroyObject(GameObject arrowClone)
    {
        yield return new WaitForEndOfFrame(); // Frame'nin sonuna kadar bekle

        DestroyImmediate(arrowClone);
    }
}
