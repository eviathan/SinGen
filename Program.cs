using System;
using System.IO;

class Program
{
    const int sampleRate = 44100;
    const int minFrequency = 2000; // Min frequency in Hz
    const int maxFrequency = 2010; // Max frequency in Hz

        static void Main()
    {
        for (int frequency = minFrequency; frequency <= maxFrequency; frequency++)
        {
            string fileName = $"sineWave_{frequency}Hz.wav";
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            using (var writer = new BinaryWriter(fileStream))
            {
                WriteWavHeader(writer, sampleRate, frequency);
                WriteSineWave(writer, frequency);
            }
        }

        Console.WriteLine("WAV files generated for each frequency.");
    }

    static void WriteWavHeader(BinaryWriter writer, int sampleRate, int frequency)
    {
        int bufferLength = sampleRate / frequency;
        int fileSize = 44 + bufferLength * 2;

        writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
        writer.Write(fileSize - 8);
        writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
        writer.Write(new char[4] { 'f', 'm', 't', ' ' });
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)1);
        writer.Write(sampleRate);
        writer.Write(sampleRate * 2);
        writer.Write((short)2);
        writer.Write((short)16);
        writer.Write(new char[4] { 'd', 'a', 't', 'a' });
        writer.Write(bufferLength * 2);
    }

    static void WriteSineWave(BinaryWriter writer, int frequency)
    {
        int bufferLength = sampleRate / frequency;
        var amplitude = 0.25 * short.MaxValue;

        for (int n = 0; n < bufferLength; n++)
        {
            var sample = (short)(amplitude * Math.Sin((2 * Math.PI * n * frequency) / sampleRate));
            writer.Write(sample);
        }
    }
}