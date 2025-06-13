namespace Behaviours.Runtime
{
    public interface IValueStorage
    {
        void SetFloat(string name, float value);

        void SetInt(string name, int value);

        void SetBool(string name, bool value);

        void SetString(string name, string value);

        float GetFloat(string name);

        int GetInt(string name);

        bool GetBool(string name);

        string GetString(string name);
    }
}