namespace Echo.JumpGates
{
	public interface IJumpGateResolver
	{
		void ResolveConnectedGate(IJumpGateRegister jumpGateRegister);
	}
}