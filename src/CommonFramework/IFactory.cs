namespace CommonFramework;

public interface IFactory<out T>
{
    T Create();
}