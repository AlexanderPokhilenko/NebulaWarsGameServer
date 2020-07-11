using System;

[Serializable]
public struct ViewTypePair // Для сериализации Unity, иначе не может
{
    public ViewTypeId key;
    public ViewTypeId value;

    public void Deconstruct(out ViewTypeId key, out ViewTypeId value)
    {
        key = this.key;
        value = this.value;
    }
}