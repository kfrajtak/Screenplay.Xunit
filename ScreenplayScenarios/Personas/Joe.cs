using CSF.FlexDi;
using CSF.Screenplay;
using CSF.Screenplay.Actors;

namespace ScreenplayScenarios.Personas
{
    public class Joe : Persona
    {
        public override string Name => "Joe";

        public override void GrantAbilities(ICanReceiveAbilities actor, IResolvesServices resolver)
        {
        }
    }
}
