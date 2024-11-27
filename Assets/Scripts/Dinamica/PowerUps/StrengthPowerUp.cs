using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerUp : PowerUp
{
    public float strengthMultiplier = 2f;

    protected override void ApplyPowerUp(GameObject player)
    {
        AccionReaccion accionReaccion = player.GetComponent<AccionReaccion>();
        if (accionReaccion != null)
        {
            accionReaccion.fuerzaMaximaEmpuje *= strengthMultiplier;
        }
    }

    protected override void RemovePowerUp(GameObject player)
    {
        AccionReaccion accionReaccion = player.GetComponent<AccionReaccion>();
        if (accionReaccion != null)
        {
            accionReaccion.fuerzaMaximaEmpuje /= strengthMultiplier;
        }
    }
}
