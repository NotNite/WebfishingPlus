using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace WebfishingPlus.Mods;

public class NetcodeImprover : IScriptMod {
    private const string ProcessNetcode = "process_netcode";
    private const string Path = "/root/WebfishingPlus";

    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var sendWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken {Name: "_send_P2P_Packet"}
        ]);
        var newlineWaiter = new TokenWaiter(t => t.Type is TokenType.Newline, waitForReady: true);

        foreach (var token in tokens) {
            if (newlineWaiter.Check(token)) {
                yield return token;

                // if $Path:
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.Dollar);
                yield return new ConstantToken(new StringVariant(Path));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, token.AssociatedData + 1);

                // if $Path.process_netcode(packet_data): return
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.Dollar);
                yield return new ConstantToken(new StringVariant(Path));
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken(ProcessNetcode);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("packet_data");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.CfReturn);
                yield return token;

                newlineWaiter.Reset();
            } else if (sendWaiter.Check(token)) {
                yield return token;
                newlineWaiter.SetReady();
            } else {
                yield return token;
            }
        }
    }
}
