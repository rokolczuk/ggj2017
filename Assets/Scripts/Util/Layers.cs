
public static class Layers 
{
	
	public static int Enemies = 15;

	public static int GetLayerMask(int layer)
	{
		return 1 << layer;
	}
}
