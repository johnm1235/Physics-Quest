using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerUp : PowerUp
{
    public float strengthMultiplier = 2f;
    private float initialStrength;

    protected override void ApplyPowerUp(GameObject player)
    {
        AccionReaccion accionReaccion = player.GetComponent<AccionReaccion>();
        if (accionReaccion != null)
        {
            initialStrength = accionReaccion.fuerzaMaximaEmpuje; // Guardar la fuerza inicial
            accionReaccion.fuerzaMaximaEmpuje *= strengthMultiplier; // Aplicar el multiplicador
        }
    }

    protected override void RemovePowerUp(GameObject player)
    {
        AccionReaccion accionReaccion = player.GetComponent<AccionReaccion>();
        if (accionReaccion != null)
        {
            accionReaccion.fuerzaMaximaEmpuje = initialStrength; // Restaurar la fuerza inicial
        }
    }
}
