public class GameManager
{
    #region

    public Player Player { get; private set; }

    public Stage Stage { get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        // TODO => Player ���ҽ� �������� �����ͼ� ���� �� �ʱ�ȭ
        // �������� ������ �����ͼ� ���� ����
        var playerClone = Manager.Resource.InstantiatePrefab("PlayerModel");
        Player = playerClone.GetComponent<Player>();
        var stageClone = Manager.Resource.InstantiatePrefab("StageController");
        Stage = stageClone.GetComponent<Stage>();
        Stage.InitStage();
    }

    #endregion
}
