public class SpellMenuSystem(IEnumerable<Spell> availableSpells) : BaseMenuSystem<Spell?>(availableSpells.Cast<Spell?>())
{
    protected override string GetTitle() => "Select a spell to cast:";

    protected override string FormatOption(Spell? spell) =>
        spell.HasValue ? $"{spell.Value.desc} (Damage: {spell.Value.spellDamage})" : "Cancel";

    protected override Spell? GetCancelValue() => null;
}
