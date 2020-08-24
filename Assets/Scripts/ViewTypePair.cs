using System;

[Serializable]
public struct ViewTypePair // Для сериализации Unity, иначе не может
{
    public ViewTypeEnum key;
    public ViewTypeEnum value;

    public void Deconstruct(out ViewTypeEnum key, out ViewTypeEnum value)
    {
        key = this.key;
        value = this.value;
    }
}