using System;
using System.Collections.Generic;
using System.Text;

namespace PkmGen1SaveEditor;

internal sealed class Gen1MoveSlot
{
    public int Slot { get; init; }

    public byte MoveId { get; init; }

    public int CurrentPp { get; init; }

    public int PpUps { get; init; }

    public bool IsEmpty => MoveId == 0;
}
