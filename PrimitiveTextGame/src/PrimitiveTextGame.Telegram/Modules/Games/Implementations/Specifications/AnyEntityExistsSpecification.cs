namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications
{
    public class AnyEntityExistsSpecification<T> : SpecificationBase<T>
    {
        public AnyEntityExistsSpecification() 
        { 
            Criteria = x => true;
        }
    }
}
