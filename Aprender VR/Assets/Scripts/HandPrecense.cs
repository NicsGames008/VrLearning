using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPrecense : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics contrllorCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handmodelPrefab;

    private InputDevice targetDevice;
    private GameObject spwanedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }


        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(contrllorCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }


        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);

            if (prefab)
            {
                spwanedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Esse manbo não esta la");

                spwanedController = Instantiate(controllerPrefabs[0], transform);

            }

            spawnedHandModel = Instantiate(handmodelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spwanedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spwanedController.SetActive(false);
                UpdateHandAnimation();
            }
        }
    }
}
