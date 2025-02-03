using System.Linq.Expressions;
using Erick_Estrada_P1_AP1.DAL;
using Erick_Estrada_P1_AP1.Models;
using Microsoft.EntityFrameworkCore;

namespace Erick_Estrada_P1_AP1.Services;

public class Services (IDbContextFactory<Contexto> DbFactory)
{
    
    public async Task<bool> Guardar(Modelo modelo)
    {
        if (!await Existe(modelo.Id))
        {
            return await Insertar(modelo);
        }
        else
        {
            return await Modificar(modelo);
        }
    }
    
    
     public async Task<bool> Existe(int Id)
    {
        await using var Contexto = await DbFactory.CreateDbContextAsync();
        return await Contexto.Modelo.AnyAsync(m=>m.Id==Id);
    }
     
    
    public async Task<bool> Insertar(Modelo modelo)
    {
        await using var Contexto  = await DbFactory.CreateDbContextAsync();
        Contexto.Add(modelo);
        return await Contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var Contexto = await DbFactory.CreateDbContextAsync();
        return await Contexto.Modelo.AsNoTracking().Where(m => m.Id==id).ExecuteDeleteAsync() >0 ;
    }

    public async Task<bool> Modificar(Modelo modelo)
    {
        await using var Contexto = await DbFactory.CreateDbContextAsync();
        Contexto.Update(modelo);
        return await Contexto.SaveChangesAsync() > 0;
    }
    
    
    public async Task<Modelo?> Buscar(int id)
    {
        await using var Contexto = await DbFactory.CreateDbContextAsync();
        return await Contexto.Modelo
            .FirstOrDefaultAsync(m=>m.Id==id);    

}

    public async Task<List<Modelo>> Listar(Expression<Func<Modelo, bool>>criterio)
    {
        await using var Contexto = await DbFactory.CreateDbContextAsync();
        return await Contexto.Modelo.Where(criterio).AsNoTracking().ToListAsync();
    }
}