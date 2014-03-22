
// How to use the model :
// make sure the Y-Vision.model is with the .exe (like the .cfg)
// using ProbabilityFunctions;
Classifier classifier = new NaiveBayesClassifier(true);
if (classifier.getNbAttributes() == 5) // check to see if the model uses the correct number of attributes, excluding the class & ID
{
	double[] row = {1,1,1,4,5};
	MessageBox.Show(classifier.Classify(row));
}