using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace PokemonAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    [Route("[controller]")]
    [ApiController]
    public class PokemonsController : ControllerBase
    {
        private readonly List<Pokemon> _pokemons = new List<Pokemon>();
        private int _nextId = 1;

        [HttpPost]
        public ActionResult<Pokemon> CreatePokemon(Pokemon pokemon)
        {
            pokemon.Id = _nextId++;
            _pokemons.Add(pokemon);
            return CreatedAtAction(nameof(GetPokemon), new { id = pokemon.Id }, pokemon);
        }

        [HttpPost("multiple")]
        public ActionResult<IEnumerable<Pokemon>> CreateMultiplePokemons(List<Pokemon> pokemons)
        {
            foreach (var pokemon in pokemons)
            {
                pokemon.Id = _nextId++;
                _pokemons.Add(pokemon);
            }
            return Ok(pokemons);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePokemon(int id, Pokemon pokemon)
        {
            var index = _pokemons.FindIndex(p => p.Id == id);
            if (index == -1)
            {
                return NotFound();
            }
            _pokemons[index] = pokemon;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePokemon(int id)
        {
            var pokemon = _pokemons.Find(p => p.Id == id);
            if (pokemon == null)
            {
                return NotFound();
            }
            _pokemons.Remove(pokemon);
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<Pokemon> GetPokemon(int id)
        {
            var pokemon = _pokemons.Find(p => p.Id == id);
            if (pokemon == null)
            {
                return NotFound();
            }
            return Ok(pokemon);
        }

        [HttpGet("tipo/{tipo}")]
        public ActionResult<IEnumerable<Pokemon>> GetPokemonsByType(string tipo)
        {
            var pokemonsByType = _pokemons.FindAll(p => p.Tipo == tipo);
            if (pokemonsByType.Count == 0)
            {
                return NotFound();
            }
            return Ok(pokemonsByType);
        }
    }

    public class Pokemon
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public List<Habilidad>? Habilidades { get; set; } = null!;
        public double Defensa { get; set; }
    }

    public class Habilidad
    {
        public string Nombre { get; set; }
        public int Valor { get; set; }
    }
}
