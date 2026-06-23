using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace NX_Macro_Controller_VxV;

public class CompletionData : ICompletionData
{
	public object Content { get; set; }

	public object Description { get; set; }

	public ImageSource Image { get; set; }

	public double Priority { get; set; }

	public string Text { get; set; }

	public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
	{
		string[] array = Text.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
		string text = "";
		for (int i = 0; i < textArea.Document.Lines[textArea.Caret.Line - 1].Length; i++)
		{
			if (textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() == " ")
			{
				text += " ";
			}
			if (textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() == "\t")
			{
				text += "\t";
			}
			if (textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() == "\u3000")
			{
				text += "\u3000";
			}
			if (textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() != "\t" && textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() != " " && textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() != "\u3000")
			{
				break;
			}
		}
		string text2 = "";
		for (int j = 0; j < array.Length; j++)
		{
			if (j != 0)
			{
				text2 += text;
			}
			text2 += array[j];
			if (j < array.Length - 1)
			{
				text2 += "\r\n";
			}
		}
		textArea.Document.Replace(completionSegment.Offset - 1, completionSegment.EndOffset - completionSegment.Offset + 1, text2);
		if (Text[Text.Length - 1] == ')')
		{
			textArea.Caret.Offset--;
		}
		if (Text[Text.Length - 1] == '}')
		{
			textArea.Caret.Line -= 3;
			textArea.Caret.Column = textArea.Document.Lines[textArea.Caret.Line - 1].Length;
		}
	}

	public CompletionData(string[] text)
	{
		Content = text[0];
		Text = text[0];
		if (text[1] == "Command" || text[1] == "Block")
		{
			Text += "()";
		}
		if (text[1] == "Block" || text[1] == "NArgsBlock")
		{
			Text = Text + Environment.NewLine + "{" + Environment.NewLine + "\t" + Environment.NewLine + "}";
		}
	}
}
