namespace CodeWithMe.Core.Dtos;

public interface DtoExtension<Dto, Entity>
{
    Dto ToDto<Dto, Entity>(Entity entity);

    Entity ToEntity<Dto, Entity>(Dto arg);
}
