   M S B A M L   `   `   `  ���� N  JMdXaml, Version=1.27.0.0, Culture=neutral, PublicKeyToken=9f8c7afb435b7edc/$clr-namespace:MdXaml;assembly=MdXamlMdXaml   TodoList   TodoList.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6mdxam$clr-namespace:MdXaml;assembly=MdXaml       � Title$  	Todo List��$	��450��$	��800�� � WindowStyle$
 None=� � 	Resources    PrimaryColor  SecondaryColor  ItemPrimary  PrimaryBrush �� Color  SecondaryBrush  	ItemBrush��%�   &	        &	      &	 "     &	 3     7   &	 =     7  &	 G     7  ��#371452����#140d1a����#140d2a���� 8   �� 8   �� 8    � DataContext ��  �� RelativeSource �� *Self+.��� -   #��[  � RowDefinitions �� $��30 ��� $��* �.+�� -   #��[ $��0��.+��� $��	Todo List��$��Center=�$��10,0,0,5q�
���iii��� -    ��Name$ PageName��$
��Left=�
������ $��0q�$
��Page��$��20(�$��Center=�$��65,0,0,5q�
���������  �� Orientation$ 
Horizontal=�$��Right=�.+��� -   $��-��$��25��$��15(�$��0q�
������ 
���iii��� -   $	��□��$��25��$��15(�$��0q�
������ 
���iii��� -   $��X��$��25��$��15(�$��0q�
������ 
���iii��� $��1��$��Hidden=�$��Hidden=�.��� #��[ .+�� $	��150��$
��Left=�.+��� -   	 ��Name$	 CurrentPageButton��$	��150��$��30��#��[ 
���iii�$	��Top=�$��0,5,0,0q����� *CurrentPage.Name+
 �� FallbackValue$

 None���� -   $	 NewPageButton��$y�Hidden=�$	��Top=�$��0,40,0,0q�$��Create New Page��$	��150��$��30��#��[ 
���iii�
 � Tag �� *Id+�� -	    ��Name$ 	PagesList��$y�Hidden=�$��0,80,0,0q����� *Pages+���� .R��� -
   $��0,5,0,0q�$	��150��$��30��#��[ 
���iii����� *Name+ �� *Id+�� -   $ PageControl��$
��Left=�$��	150,0,0,0q����� *CurrentPage.Groupings+���� .R��� $ 
Horizontal=����� .R��� ���� � -    �Name$ PropertySelector��$y�Hidden=�$��150,15,150,15q� �� $
��Auto ��� $��* �.+��� #��[ $��3q�._�� $��60��#��[ .+��� -   $ ListName��$��32(�
������  �� 	MaxLength$ 20��
�������$��0q�$��2,0,0,2q�$��Center=�$
��Left=����� *,*CurrentPage.CurrentItem.ViewModel.ListName+$

 None���� -   $ ShortDescription��$��18(�
������ $ 23��
�������$��0q�$��0,0,0,2q�$��Bottom=�$��Right=����� *42CurrentPage.CurrentItem.ViewModel.ShortDescription+$

 None���� -   $��X��$��25��$��20(�$��0q�
������ 
���iii�$��Right=�$	��Top=��� -   $��Edit Markdown��$��81��$��12(�$��0q�
������ 
���iii�$
��Left=�$��Bottom=��� $��1��#��[ $��3q�._�� #��[ .+�!   MdXaml.MarkdownScrollViewer  2  ,System.Windows.Markup.XmlAttributeProperties  XmlSpace preserve-     Name$ MarkdownEditor��" ,� VerticalScrollBarVisibility$ Hidden=�$ ,� HorizontalScrollBarVisibility$ Hidden=�
�������$y�Visible=�   Markdown �� �� $��Disabled=�$��Hidden=�.���� -   $ Editor��$��12(�
������ 
�������$y�Hidden=� �� TextWrapping$
 Wrap=� �� AcceptsReturn$
 True��