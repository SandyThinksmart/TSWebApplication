using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Domain.Model
{
    public class GoogleTextDetection
    {
        public List<Response> Responses { get; set; }
    }

    public class Response
    {
        public List<TextAnnotation> TextAnnotations { get; set; }
        public FullTextAnnotation FullTextAnnotation { get; set; }
    }

    // -------- fullTextAnnotation --------
    public class FullTextAnnotation
    {
        public string Text { get; set; }
        public List<Page> Pages { get; set; }
    }

    public class Page
    {
        public List<Block> Blocks { get; set; }
    }

    public class Block
    {
        public List<Paragraph> Paragraphs { get; set; }
    }

    public class Paragraph
    {
        public List<Word> Words { get; set; }
    }

    public class Word
    {
        public BoundingPoly BoundingBox { get; set; }
        public List<Symbol> Symbols { get; set; }
    }

    public class Symbol
    {
        public string Text { get; set; }
        public BoundingPoly BoundingBox { get; set; }
    }

    // -------- textAnnotations --------
    public class TextAnnotation
    {
        public string Description { get; set; }
        public BoundingPoly BoundingPoly { get; set; }
    }

    // -------- shared types --------
    public class BoundingPoly
    {
        public List<Vertex> Vertices { get; set; }
    }

    public class Vertex
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

public class WordsAnnotations
{
    public string wordText { get; set; }
    public List<Vertex> Vertices { get; set; }
    public string Data { get; set; }
    public BoundingPoly BoundingBox { get; set; }

}