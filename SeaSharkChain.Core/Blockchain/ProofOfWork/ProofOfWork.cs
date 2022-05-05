namespace SeaSharkChain.Core;

public static class ProofOfWork
{
    public static ProofOfWorkCalculateResult Calculate(string blockHashWithoutPow, int difficulty)
    {
        string difficultyString = DifficultyString(difficulty);
        int nonce = 0;
        while (true)
        {
            var toHash = $"{nonce}{blockHashWithoutPow}";
            string hashedData = ShaHasher.ComputeSha2_512(toHash).StringIn64Base;

            if (hashedData.StartsWith(difficultyString, StringComparison.Ordinal))
                return new ProofOfWorkCalculateResult { HashIn64Base = hashedData, Nonce = nonce };

            nonce++;
        }
    }

    public static bool ValidateForNonce(string blockHashWithoutPow, int difficulty, int nonce, string expectedPowHash)
    {
        string difficultyString = DifficultyString(difficulty);

        if (!expectedPowHash.StartsWith(difficultyString))
            return false;

        var toHash = $"{nonce}{blockHashWithoutPow}";
        string hashedData = ShaHasher.ComputeSha2_512(toHash).StringIn64Base;

        return hashedData.Equals(expectedPowHash);
    }

    private static string DifficultyString(int difficulty)
    {
        string difficultyString = string.Empty;

        for (int i = 0; i < difficulty; i++)
        {
            difficultyString += "0";
        }

        return difficultyString;
    }
}

