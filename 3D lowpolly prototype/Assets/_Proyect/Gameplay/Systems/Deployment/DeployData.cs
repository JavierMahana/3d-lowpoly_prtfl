using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public struct DeployData
{
    //la data podria tener referencias a el modelo, la carta y si esta mostrando el modelo. sos.

    public bool ShowingCard;
    public DeploymentCard DeploymentCard;
    public DeploymentPreview Preview;
    public int Index;

    public DeployData(DeploymentCard card, DeploymentPreview preview, int index, bool showignCard = true)
    {
        DeploymentCard = card;
        ShowingCard = showignCard;
        Preview = preview;
        Index = index;
    }
}

    //public struct DeployData 
    //{
    //    //la data podria tener referencias a el modelo, la carta y si esta mostrando el modelo. sos.

    //    public bool ShowingCard;
    //    public DeploymentCard DeploymentCard;
    //    public GameObject Preview;


    //    public DeployData(DeploymentCard card, GameObject preview, bool showignCard = true)
    //    {
    //        DeploymentCard = card;
    //        ShowingCard = showignCard;
    //        Preview = preview;
    //    }
//}
