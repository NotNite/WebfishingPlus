using GDWeave.Godot;
using GDWeave.Modding;

namespace WebfishingPlus.Mods;

public class MeteorNotice : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/World/world.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var spawnFishMatch = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken {Name: "_spawn_fish"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken {Name: "type"},
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken {Name: "fish_spawn"},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline && t.AssociatedData is 1,
        ]);

        foreach (var token in tokens) {
            if (spawnFishMatch.Check(token)) {
                yield return token;

                // if type == "fish_alien": PlayerData._send_notification("...")
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("type");
                yield return new Token(TokenType.OpEqual);
                yield return new IdentifierToken("fish_alien");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_send_notification");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("you notice something fall out of the sky...");
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 1);
            } else {
                yield return token;
            }
        }
    }
}
